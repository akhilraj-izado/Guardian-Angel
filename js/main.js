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
  const currentPath = window.location.pathname.split('/').pop().split('?')[0] || 'index.html';

  // Desktop Links Active State
  document.querySelectorAll('.nav-link').forEach(link => {
    const linkHref = link.getAttribute('href');

    const isCurrentPage =
      linkHref === currentPath ||
      (currentPath === '' && linkHref === 'index.html');

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
    const linkHref = link.getAttribute('href');
    if (linkHref === currentPath || (currentPath === '' && linkHref === 'index.html')) {
      link.classList.add('text-sky', 'pl-1');
      link.classList.remove('text-navy/80');
    }
  });
});