import { Routes } from "@angular/router";

export const productsRoutes: Routes = [
    { path: 'products', loadComponent: () => import('./product-list/product-list').then(m => m.ProductList) },
    { path: 'product/new', loadComponent: () => import('./product-actions/product-actions').then(m => m.ProductActions) },
    { path: 'product/:id', loadComponent: () => import('./product-actions/product-actions').then(m => m.ProductActions) },
    { path: 'product/:id/edit', loadComponent: () => import('./product-actions/product-actions').then(m => m.ProductActions) }
]