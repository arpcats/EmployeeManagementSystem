import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'employees', pathMatch: 'full' },
  {
    path: 'employees',
    loadComponent: () => import('./features/employee-list/employee-list').then(m => m.EmployeeList)
  },
  {
    path: 'employees/new',
    loadComponent: () => import('./features/employee-form/employee-form').then(m => m.EmployeeForm)
  },
  {
    path: 'employees/:id/edit',
    loadComponent: () => import('./features/employee-form/employee-form').then(m => m.EmployeeForm)
  },
  { path: '**', redirectTo: 'employees' }
];
