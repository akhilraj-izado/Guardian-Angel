document.addEventListener('DOMContentLoaded', () => {
  // 1. Mobile Menu Toggle
  const menuBtn = document.getElementById('menuBtn');
  const mobileMenu = document.getElementById('mobileMenu');

  if (menuBtn && mobileMenu) {
    menuBtn.addEventListener('click', () => {
      const isExpanded = menuBtn.getAttribute('aria-expanded') === 'true';
      menuBtn.setAttribute('aria-expanded', !isExpanded);
      mobileMenu.classList.toggle('hidden');
    });
  }

  // 2. Auto-Detect Current Page
  const normalize = path => path.replace(/\/+$/, '').toLowerCase() || '/';
  const currentPath = normalize(window.location.pathname);
  const isCurrent = link => normalize(new URL(link.href, window.location.origin).pathname) === currentPath;

  // Desktop Links Active State
  document.querySelectorAll('.nav-link').forEach(link => {
    const isCurrentPage = isCurrent(link);

    link.classList.toggle('active', isCurrentPage);

    if (isCurrentPage) {
      link.classList.remove('border-transparent', 'text-navy/80');
      link.classList.add('text-navy');
    } else {
      link.classList.remove('border-navy', 'text-navy');
      link.classList.add('border-transparent', 'text-navy/80');
    }
  });

  // Mobile Links Active State
  document.querySelectorAll('.mobile-nav-link').forEach(link => {
    if (isCurrent(link)) {
      link.classList.add('text-sky', 'pl-1');
      link.classList.remove('text-navy/80');
    }
  });
});