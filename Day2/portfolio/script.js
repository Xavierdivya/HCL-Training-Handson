/* ==========================================================================
   Divya X — Portfolio behaviour
   Theme • landing "enter" interaction • orb pointer parallax • navigation
   • mobile menu • scroll reveals • active-section tracking
   Everything degrades gracefully: with JavaScript off, all content is visible.
   ========================================================================== */
(function () {
  'use strict';

  var root = document.documentElement;
  var body = document.body;
  var $ = function (sel, ctx) { return (ctx || document).querySelector(sel); };
  var $$ = function (sel, ctx) { return Array.prototype.slice.call((ctx || document).querySelectorAll(sel)); };

  var reduceMotionQuery = window.matchMedia ? window.matchMedia('(prefers-reduced-motion: reduce)') : { matches: false };
  var prefersReducedMotion = function () { return reduceMotionQuery.matches; };

  /* ---------- Theme (light / dark) ---------- */
  var themeBtn = $('#themeToggle');
  var themeMeta = $('meta[name="theme-color"]');
  var THEME_COLORS = { light: '#faf7f2', dark: '#0a0f1f' };

  function storedTheme() {
    try { var t = localStorage.getItem('theme'); return (t === 'light' || t === 'dark') ? t : null; }
    catch (e) { return null; }
  }

  function applyTheme(theme) {
    root.setAttribute('data-theme', theme);
    if (themeBtn) {
      themeBtn.setAttribute('aria-pressed', String(theme === 'dark'));
      themeBtn.setAttribute('aria-label', theme === 'dark' ? 'Switch to light theme' : 'Switch to dark theme');
    }
    if (themeMeta) themeMeta.setAttribute('content', THEME_COLORS[theme]);
  }

  applyTheme(root.getAttribute('data-theme') === 'dark' ? 'dark' : 'light');

  if (themeBtn) {
    themeBtn.addEventListener('click', function () {
      var next = root.getAttribute('data-theme') === 'dark' ? 'light' : 'dark';
      applyTheme(next);
      try { localStorage.setItem('theme', next); } catch (e) { /* storage unavailable */ }
    });
  }

  // Follow the system theme live until the visitor makes their own choice.
  if (window.matchMedia) {
    var darkQuery = matchMedia('(prefers-color-scheme: dark)');
    var onSystemChange = function (e) { if (!storedTheme()) applyTheme(e.matches ? 'dark' : 'light'); };
    if (darkQuery.addEventListener) darkQuery.addEventListener('change', onSystemChange);
    else if (darkQuery.addListener) darkQuery.addListener(onSystemChange);
  }

  /* ---------- Landing: enter the portfolio ---------- */
  var orb = $('#orb');
  var hero = $('#home');
  var heroExtra = $('#heroExtra');
  var heroHint = $('#heroHint');
  var entered = false;

  // Before entering, the intro/CTA block is hidden visually, so hide it from keyboards and screen readers too.
  function setExtraAvailable(available) {
    if (!heroExtra) return;
    if (available) { heroExtra.removeAttribute('inert'); heroExtra.removeAttribute('aria-hidden'); }
    else { heroExtra.setAttribute('inert', ''); heroExtra.setAttribute('aria-hidden', 'true'); }
  }
  setExtraAvailable(false);

  function enterPortfolio(instant) {
    if (entered) return;
    entered = true;
    if (instant || prefersReducedMotion()) body.classList.add('instant');
    else body.classList.add('entering');
    body.classList.add('entered');
    setExtraAvailable(true);
    if (orb) orb.setAttribute('aria-label', 'Scroll to the About section');
    if (heroHint) heroHint.textContent = 'Scroll to explore';
    updateActive();
    setTimeout(function () { body.classList.remove('entering'); }, 1100);
  }

  function scrollToId(id, updateHash) {
    var target = id === 'home' ? hero : document.getElementById(id);
    if (!target) return;
    var smooth = !prefersReducedMotion();
    if (id === 'home') window.scrollTo({ top: 0, behavior: smooth ? 'smooth' : 'auto' });
    else target.scrollIntoView({ behavior: smooth ? 'smooth' : 'auto', block: 'start' });
    if (updateHash && window.history && history.replaceState) {
      try { history.replaceState(null, '', id === 'home' ? location.pathname + location.search : '#' + id); } catch (e) { /* file:// quirks */ }
    }
  }

  if (orb) {
    orb.addEventListener('click', function () {
      if (!entered) enterPortfolio(false);
      else scrollToId('about', true);
    });
  }

  /* ---------- Orb: subtle pointer parallax ---------- */
  if (orb && hero && !prefersReducedMotion()) {
    var frame = null, nextX = 0, nextY = 0;
    var paint = function () {
      orb.style.setProperty('--px', nextX.toFixed(3));
      orb.style.setProperty('--py', nextY.toFixed(3));
      frame = null;
    };
    hero.addEventListener('pointermove', function (e) {
      var r = hero.getBoundingClientRect();
      nextX = Math.max(-1, Math.min(1, ((e.clientX - r.left) / r.width - 0.5) * 2));
      nextY = Math.max(-1, Math.min(1, ((e.clientY - r.top) / r.height - 0.5) * 2));
      if (!frame) frame = requestAnimationFrame(paint);
    });
    hero.addEventListener('pointerleave', function () {
      nextX = 0; nextY = 0;
      if (!frame) frame = requestAnimationFrame(paint);
    });
  }

  /* ---------- Navigation (all in-page links) ---------- */
  var menuBtn = $('#menuToggle');
  var mobileMenu = $('#mobileMenu');

  function setMenu(open) {
    body.classList.toggle('menu-open', open);
    if (menuBtn) {
      menuBtn.setAttribute('aria-expanded', String(open));
      menuBtn.setAttribute('aria-label', open ? 'Close menu' : 'Open menu');
    }
    if (open && mobileMenu) {
      var first = $('a', mobileMenu);
      if (first) setTimeout(function () { first.focus(); }, 60);
    }
  }

  if (menuBtn) {
    menuBtn.addEventListener('click', function () { setMenu(!body.classList.contains('menu-open')); });
  }
  document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape' && body.classList.contains('menu-open')) { setMenu(false); if (menuBtn) menuBtn.focus(); }
  });
  // Close the overlay if the viewport grows past the mobile breakpoint.
  window.addEventListener('resize', function () {
    if (window.innerWidth >= 992 && body.classList.contains('menu-open')) setMenu(false);
  });

  $$('a[data-nav]').forEach(function (link) {
    link.addEventListener('click', function (e) {
      var href = link.getAttribute('href') || '';
      if (href.charAt(0) !== '#') return;
      var id = href.slice(1);
      if (!document.getElementById(id)) return;
      e.preventDefault();
      var wasOpen = body.classList.contains('menu-open');
      if (wasOpen) setMenu(false);
      if (!entered && id !== 'home') {
        enterPortfolio(false);
        // Give the newly displayed sections a moment to lay out before scrolling.
        setTimeout(function () { scrollToId(id, true); }, prefersReducedMotion() ? 0 : 120);
      } else {
        scrollToId(id, true);
      }
    });
  });

  /* ---------- Top bar background after scrolling ---------- */
  var topbar = $('#topbar');
  function onScroll() { if (topbar) topbar.classList.toggle('is-scrolled', window.scrollY > 12); }
  window.addEventListener('scroll', onScroll, { passive: true });
  onScroll();

  /* ---------- Active section indicator ---------- */
  var sectionIds = ['home', 'about', 'skills', 'experience', 'projects', 'education', 'achievements', 'contact'];
  var navLinks = $$('a[data-nav][href^="#"]').filter(function (a) { return a.closest('.rail-nav') || a.closest('.mobile-menu'); });
  var currentId = 'home';

  function setActive(id) {
    currentId = id;
    navLinks.forEach(function (a) {
      if (a.getAttribute('href') === '#' + id) a.setAttribute('aria-current', 'true');
      else a.removeAttribute('aria-current');
    });
  }

  function updateActive() {
    if (!entered) { setActive('home'); return; }
    var probe = window.innerHeight * 0.4, active = 'home';
    sectionIds.forEach(function (id) {
      var el = document.getElementById(id);
      if (el && el.getBoundingClientRect().top <= probe) active = id;
    });
    // Near the very bottom, the last section should win even if it is short.
    if (window.innerHeight + window.scrollY >= document.documentElement.scrollHeight - 4) active = 'contact';
    if (active !== currentId) setActive(active);
  }

  var ticking = false;
  window.addEventListener('scroll', function () {
    if (ticking) return;
    ticking = true;
    requestAnimationFrame(function () { updateActive(); ticking = false; });
  }, { passive: true });
  window.addEventListener('resize', updateActive);
  setActive('home');

  /* ---------- Scroll-triggered reveals ---------- */
  var revealEls = $$('.reveal');
  if ('IntersectionObserver' in window) {
    var io = new IntersectionObserver(function (entries) {
      entries.forEach(function (entry) {
        if (entry.isIntersecting) { entry.target.classList.add('in'); io.unobserve(entry.target); }
      });
    }, { threshold: 0.12, rootMargin: '0px 0px -6% 0px' });
    revealEls.forEach(function (el) { io.observe(el); });
  } else {
    revealEls.forEach(function (el) { el.classList.add('in'); });
  }

  /* ---------- Deep links skip the intro gate ---------- */
  var initialHash = location.hash ? location.hash.slice(1) : '';
  if (initialHash && initialHash !== 'home' && document.getElementById(initialHash)) {
    enterPortfolio(true);
    // Wait for layout, then jump to the section.
    window.addEventListener('load', function () { scrollToId(initialHash, false); });
  }

  /* ---------- Footer year ---------- */
  var yearEl = $('#year');
  if (yearEl) yearEl.textContent = String(new Date().getFullYear());
})();
