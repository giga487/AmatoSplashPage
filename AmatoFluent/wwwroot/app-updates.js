(() => {
    if (!('serviceWorker' in navigator)) return;

    const notice = document.getElementById('app-update');
    const button = document.getElementById('app-update-button');
    let reloading = false;

    navigator.serviceWorker.addEventListener('controllerchange', () => {
        // Other tabs also reload when an update is activated, keeping code and assets together.
        if (navigator.serviceWorker.controller && !reloading) {
            reloading = true;
            window.location.reload();
        }
    });

    navigator.serviceWorker.register('service-worker.js', { updateViaCache: 'none' })
        .then(registration => {
            const showUpdate = () => {
                if (registration.waiting && navigator.serviceWorker.controller) {
                    notice.hidden = false;
                }
            };
            showUpdate();
            registration.addEventListener('updatefound', () => {
                registration.installing?.addEventListener('statechange', showUpdate);
            });
            button.addEventListener('click', () => {
                if (registration.waiting) {
                    button.disabled = true;
                    button.textContent = 'Updating…';
                    registration.waiting.postMessage({ type: 'ACTIVATE_UPDATE' });
                } else {
                    window.location.reload();
                }
            });
        })
        .catch(error => console.warn('Site update check failed:', error));
})();
