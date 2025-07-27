import { Routes } from '@angular/router';

export const customerRoutes: Routes = [
    { path: 'customers', loadComponent: () => import('./customer-list/customer-list').then(m => m.CustomerList) },
    { path: 'customer/new', loadComponent: () => import('./customer-actions/customer-actions').then(m => m.CustomerActions) },
    { path: 'customer/:id', loadComponent: () => import('./customer-actions/customer-actions').then(m => m.CustomerActions) },
    { path: 'customer/:id/edit', loadComponent: () => import('./customer-actions/customer-actions').then(m => m.CustomerActions) },
]