import { Routes } from '@angular/router';
import { categoriesRoutes } from './Category/categories.routes';
import { productsRoutes } from './Product/products.routes';
import { customerRoutes } from './Customer/customer.routes';
import { invoicesRoutes } from './Invoice/invoices.routes';

export const routes: Routes = [
    // { path: '', loadComponent: () => import('./app').then(m => m.App), pathMatch: 'full' },
    { path: '', redirectTo: 'categories', pathMatch: 'full' },
    ...categoriesRoutes,
    ...productsRoutes,
    ...invoicesRoutes,
    ...customerRoutes,
    { path: 'invoice/:id', loadComponent: () => import('./Invoice/invoice-details/invoice-details').then(m => m.InvoiceDetails) },
    { path: '**', loadComponent: () => import('./not-found/not-found').then(m => m.NotFound) }
];
