import { Routes } from "@angular/router";

export const invoicesRoutes: Routes = [
    { path: 'invoices', loadComponent: () => import('./invoice-list/invoice-list').then(m => m.InvoiceList) },
    { path: 'invoices/:id', loadComponent: () => import('./invoice-details/invoice-details').then(m => m.InvoiceDetails) }
]