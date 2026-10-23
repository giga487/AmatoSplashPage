const assert = require('node:assert/strict');
const { readFileSync } = require('node:fs');
const { resolve } = require('node:path');
const { runInNewContext } = require('node:vm');
const { test } = require('node:test');

const root = resolve(__dirname, '../AmatoFluent/wwwroot');

test('versioned Fluent imports stay in the worker snapshot after deployment', async () => {
    const listeners = {};
    let activated = false;
    const stored = new Map([
        ['https://example.com/AmatoSplashPage/_content/Fluent/Divider.js', 'old module'],
        ['https://example.com/AmatoSplashPage/css/app.css', 'old CSS'],
        ['https://example.com/AmatoSplashPage/sample-data/cv-data.json', 'cached CV'],
        ['https://example.com/AmatoSplashPage/index.html', 'cached shell']
    ]);
    runInNewContext(readFileSync(resolve(root, 'service-worker.published.js'), 'utf8'), {
        URL,
        self: {
            location: { href: 'https://example.com/AmatoSplashPage/service-worker.js' },
            importScripts() {},
            assetsManifest: { version: 'old', assets: [] },
            addEventListener: (name, callback) => { listeners[name] = callback; },
            skipWaiting: async () => { activated = true; }
        },
        caches: { open: async () => ({ match: async (request, options) => {
            const url = new URL(typeof request === 'string' ? request : request.url);
            if (options.ignoreSearch) url.search = '';
            return stored.get(url.href);
        } }) },
        fetch: async () => 'network',
        console
    });
    async function fetchAsset(path, destination, mode = 'cors') {
        let response;
        listeners.fetch({ request: { url: 'https://example.com/AmatoSplashPage/' + path,
            method: 'GET', destination, mode }, respondWith(value) { response = value; } });
        return response;
    }
    assert.equal(await fetchAsset('_content/Fluent/Divider.js?v=4.11.9', 'script'), 'old module');
    assert.equal(await fetchAsset('css/app.css?v=old', 'style'), 'old CSS');
    assert.equal(await fetchAsset('sample-data/cv-data.json?fresh=1', ''), 'network');
    assert.equal(await fetchAsset('cv', 'document', 'navigate'), 'cached shell');
    assert.equal(activated, false, 'No automatic activation during normal use');
    let activation;
    listeners.message({ data: { type: 'ACTIVATE_UPDATE' }, waitUntil(value) { activation = value; } });
    await activation;
    assert.equal(activated, true);
});

test('a waiting update is offered and activates only after clicking the button', async () => {
    const listeners = {};
    const elements = {
        'app-update': { hidden: true },
        'app-update-button': { addEventListener: (name, callback) => { listeners.click = callback; } }
    };
    let message;
    let reloads = 0;
    const worker = { postMessage: value => { message = value.type; } };
    const registration = { waiting: worker, addEventListener() {} };
    runInNewContext(readFileSync(resolve(root, 'app-updates.js'), 'utf8'), {
        navigator: { serviceWorker: {
            controller: {},
            addEventListener: (name, callback) => { listeners[name] = callback; },
            register: async (url, options) => {
                assert.equal(url, 'service-worker.js');
                assert.equal(options.updateViaCache, 'none');
                return registration;
            }
        } },
        document: { getElementById: id => elements[id] },
        window: { location: { reload: () => { reloads++; } } },
        console
    });
    await Promise.resolve();
    assert.equal(elements['app-update'].hidden, false);
    assert.equal(message, undefined);
    listeners.click();
    assert.equal(message, 'ACTIVATE_UPDATE');
    assert.equal(elements['app-update-button'].disabled, true);
    listeners.controllerchange();
    listeners.controllerchange();
    assert.equal(reloads, 1);
});
