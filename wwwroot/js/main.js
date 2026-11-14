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

  const onProjectPage = currentPath.startsWith('/project');

  // Desktop Links Active State (same sky colour as hover)
  document.querySelectorAll('.nav-link').forEach(link => {
    // The Projects dropdown button has no href: mark it active on any /project page
    const isCurrentPage = link.hasAttribute('href') ? isCurrent(link) : onProjectPage;

    link.classList.toggle('active', isCurrentPage);

    if (isCurrentPage) {
      link.classList.remove('border-transparent', 'text-navy/80', 'text-navy');
      link.classList.add('text-sky', 'border-sky');
    } else {
      link.classList.remove('border-sky', 'text-sky');
      link.classList.add('border-transparent', 'text-navy/80');
    }
  });

  // Mobile Links Active State
  document.querySelectorAll('.mobile-nav-link, #mobileMenu a[href^="/project/"]').forEach(link => {
    if (isCurrent(link)) {
      link.classList.add('text-sky', 'pl-1');
      link.classList.remove('text-navy/80');
    }
  });
  if (onProjectPage) {
    document.getElementById('mobileProjectsBtn')?.classList.add('text-sky');
  }
});