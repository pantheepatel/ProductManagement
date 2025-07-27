import { Routes } from '@angular/router';

export const categoriesRoutes: Routes = [
    { path: 'categories', loadComponent: () => import('./category-list/category-list').then(m => m.CategoryList) },
    { path: 'category/new', loadComponent: () => import('./category-actions/category-actions').then(m => m.CategoryActions) },
    { path: 'category/:id', loadComponent: () => import('./category-actions/category-actions').then(m => m.CategoryActions) },
    { path: 'category/:id/edit', loadComponent: () => import('./category-actions/category-actions').then(m => m.CategoryActions) }
];
