export const site = {
  name: 'Ali Hussain',
  role: 'Full-Stack Developer | C# / .NET Developer',
  headline: 'Building Reliable Software From Database to User Interface.',
  tagline:
    'I build reliable full-stack applications, business systems, and modern web experiences using .NET, C#, SQL Server, React, and modern frontend technologies.',
  email: 'alihussainkhushimuhammad@gmail.com',
  social: {
    github: 'https://github.com/alihussain1129',
    linkedin: 'https://www.linkedin.com/in/alihussain1129',
  },
  about: [
    'I’m a Computer Science student and self-taught developer who enjoys turning ideas into practical software. My main focus is C# and .NET development, while also building modern frontend experiences with React and JavaScript.',
    'I’m particularly interested in business applications, database-driven systems, clean architecture, and software that solves real problems.',
  ],
  skills: [
    'C#',
    '.NET',
    'ASP.NET Core',
    'React',
    'JavaScript',
    'WPF',
    'SQL Server',
    'REST APIs',
    'EF Core',
    'UI/UX',
    'System Design',
    'Software Architecture',
  ],
  techBadges: ['C# / .NET', 'React', 'WPF / MVVM', 'SQL Server', 'Git / GitHub'],
  services: [
    {
      title: 'Full-Stack Web Development',
      description:
        'Design and build modern web applications for businesses that need clean workflows, secure data handling, and scalable user experiences.',
      points: ['React interfaces', 'REST APIs', 'Business workflow logic'],
    },
    {
      title: '.NET Application Development',
      description:
        'Create robust backend systems, internal tools, and enterprise-ready services using the .NET ecosystem and SQL Server.',
      points: ['ASP.NET Core APIs', 'Database integration', 'Maintainable architecture'],
    },
    {
      title: 'WPF Desktop Applications',
      description:
        'Develop professional Windows desktop applications using C#, WPF, XAML, MVVM, and data binding.',
      points: ['WPF / XAML', 'MVVM structure', 'Windows workflows'],
    },
    {
      title: 'Database Development',
      description:
        'Design and integrate SQL Server and MySQL databases with applications using ADO.NET and Entity Framework Core.',
      points: ['Relational design', 'EF Core', 'Reliable integrations'],
    },
    {
      title: 'Business Management Systems',
      description:
        'Build customized systems for employee management, attendance, salaries, departments, authentication, and reporting.',
      points: ['Business workflows', 'Authentication', 'Reporting'],
    },
  ],
  projects: [
    {
      id: 1,
      slug: 'employee-management-system',
      title: 'Employee Management System',
      summary:
        'A database-driven employee management application designed to manage employees, departments, salaries, attendance, authentication, and business records through a structured desktop interface.',
      techStack: 'C#, .NET, WPF, MVVM, SQL Server, ADO.NET',
      isFeatured: true,
      link: 'https://github.com/alihussain1129/EmployeeManagementSystem',
      liveUrl: 'https://github.com/alihussain1129/EmployeeManagementSystem',
    },
  ],
  courses: [
    {
      slug: 'modern-web-development-foundations',
      title: 'Modern Web Development Foundations',
      description:
        'A practical course covering frontend architecture, responsive design, and the building blocks of modern application interfaces.',
      duration: '4 weeks',
      level: 'Beginner',
      lessons: 12,
    },
    {
      slug: 'dotnet-api-development',
      title: '.NET API Development',
      description:
        'Learn how to design scalable APIs, connect them to SQL Server, and structure business logic for maintainable services.',
      duration: '5 weeks',
      level: 'Intermediate',
      lessons: 16,
    },
    {
      slug: 'database-design-for-software',
      title: 'Database Design for Software',
      description:
        'Understand relational data modeling, constraints, indexing, and how to design reliable systems around business rules.',
      duration: '3 weeks',
      level: 'Intermediate',
      lessons: 10,
    },
  ],
  resources: [
    {
      id: 'clean-architecture-checklist',
      title: 'Clean Architecture Checklist',
      description: 'A practical checklist for building maintainable, testable software systems.',
      type: 'PDF',
      pageCount: 8,
    },
    {
      id: 'sql-server-optimization-guide',
      title: 'SQL Server Optimization Guide',
      description: 'A resource for understanding indexing, performance tuning, and query design.',
      type: 'PDF',
      pageCount: 11,
    },
  ],
  nav: [
    { label: 'Home', to: '/' },
    { label: 'About', to: '/about' },
    { label: 'Skills', to: '/about#skills' },
    { label: 'Services', to: '/services' },
    { label: 'Projects', to: '/projects' },
    { label: 'Documents', to: '/courses' },
    { label: 'Contact', to: '/contact' },
  ],
}