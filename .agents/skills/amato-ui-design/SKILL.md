---
name: amato-ui-design
description: Design, refine, or review the AmatoSplashPage portfolio UI in Blazor WebAssembly and Fluent UI, adapting Emil Kowalski's design principles to this project's pages, CSS, and interactions.
---

# Amato UI Design

## Scope and provenance

This is a project-specific adaptation, not an official Emil Kowalski skill or a copy of his website. Use it for visual design and interface interactions in this repository. It does not define a fixed template or authorize publishing changes.

Official references consulted on 2026-10-10:
- [Design engineering](https://github.com/emilkowalski/skills/blob/main/skills/emil-design-eng/SKILL.md): component feedback and motion decisions.
- [Mobile native](https://github.com/emilkowalski/skills/blob/main/skills/mobile-native/SKILL.md): touch and viewport behavior.
- [Sonner CSS](https://github.com/emilkowalski/sonner/blob/main/src/styles.css): a concrete example of neutral color scales, surfaces, and focus styling.

Read these only when deeper guidance is needed. Treat framework-specific examples as references to translate, not dependencies to install.

## Project context

The app lives in `AmatoFluent/`: shared layout in `Layout/MainLayout.razor`, navigation in `Layout/NavMenu.razor`, pages in `Pages/`, shared styles in `wwwroot/css/app.css`.

Keep Blazor WebAssembly, Fluent UI, and the SkiaSharp demos. Do not introduce React to reproduce a visual effect. Inspect Fluent component rendering before assuming external CSS reaches its internal elements.

The CV reads `wwwroot/sample-data/cv-data.json`, copied from root `cv-data.json` during builds. Preserve the source of truth. GitHub Pages publishes under `/AmatoSplashPage/`; preserve base-relative navigation and assets when editing the UI.

## Proposed visual direction

The user wants to improve the default Blazor appearance. As an initial project direction, favor a readable personal portfolio: left-aligned introduction, controlled content width, clear type hierarchy, neutral surfaces, and a restrained accent. These are local proposals, adjustable when the user chooses a different style.

Prioritize identity, work, and contact information on the home page. Let demos show the experimental side of the portfolio. Give the CV a reading rhythm instead of treating every section as an equally prominent card. Choose spacing and surface tokens shared across pages; reuse existing Fluent tokens where practical.

## Interaction guidance

Choose motion by purpose and frequency. Use short feedback for presses; avoid decorative delays on repeated or keyboard actions. Prefer retargetable CSS transitions for state changes. Name the animated properties explicitly and favor transform and opacity for movement. Typical starting points are 100-160 ms for presses and 150-250 ms for small reveals; tune them to the component. Keep content usable during transitions.

Gate hover effects with `(hover: hover) and (pointer: fine)`. Provide visible keyboard focus and touch feedback. Respect `prefers-reduced-motion` by reducing positional movement. Match motion to the component rather than applying one effect everywhere.

For this project, keep gameplay input immediate. UI motion guidance concerns surrounding controls, not the frame-by-frame Skia simulation. Consider a pause control for automatic decorative demos when relevant.

Use viewport units and safe-area spacing according to the layout. Preserve zoom. Make mobile navigation comfortably tappable and expose its expanded state with accessible button semantics.

## Working and verification

Inspect the affected page and shared layout first. Separate observed defects from proposed visual choices. For a review, report an actual table with Before, After, and Why columns; proposals must not be described as completed changes.

Implement the requested scope and check related routes, focus, and responsive behavior. Build the app after Razor changes. Check browser rendering at desktop and narrow widths when tools are available. For touch behavior, distinguish code inspection or emulation from validation on a physical phone.

A build can update the generated CV copy: inspect the diff and restore only changes caused by your own verification. Report what changed, what was checked, and any unverified visual behavior.