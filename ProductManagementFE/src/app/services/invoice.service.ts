import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable, of } from "rxjs";
import { InvoiceCreateDTO, InvoiceModel } from "../core/models/invoice.model";

@Injectable({ providedIn: 'root' })
export class InvoiceService {

    private apiUrl = `${environment.baseUrl}api/Invoice`;

    constructor(private http: HttpClient) { }

    // GET: invoice list
    getInvoices(): Observable<InvoiceModel[]> {
        return this.http.get<InvoiceModel[]>(`${this.apiUrl}`);
        // return of([
        //     {
        //         id: 'INV-001',
        //         customerId: 'CUST-001',
        //         date: new Date('2025-07-10'),
        //         grandTotal: 2360,
        //         taxTotal: 360,
        //         priceTotal: 2000,
        //         totalItems: 2,
        //         items: [] // overview doesn't include items
        //     },
        //     {
        //         id: 'INV-002',
        //         customerId: 'CUST-002',
        //         date: new Date('2025-07-12'),
        //         grandTotal: 1380,
        //         taxTotal: 180,
        //         priceTotal: 1200,
        //         totalItems: 4,
        //         items: [] // overview only
        //     }
        // ]);
    }

    // CREATE: add new invoice
    addInvoice(invoice: InvoiceCreateDTO): Observable<InvoiceCreateDTO> {
        return this.http.post<InvoiceCreateDTO>(`${this.apiUrl}`, invoice);
        // return of(invoice); // Simulating a successful response
    }

    // GET: invoice details by ID
    getInvoiceById(id: string): Observable<InvoiceModel> {
        return this.http.get<InvoiceModel>(`${this.apiUrl}/${id}`);
        // return of({
        //     id: 'INV-001',
        //     customerId: 'CUST-001',
        //     date: new Date('2025-07-10'),
        //     grandTotal: 2360,
        //     taxTotal: 360,
        //     priceTotal: 2000,
        //     totalItems: 2,
        //     items: [
                
        //     ]
        // });
    }

    // UPDATE: edit specific invoice
    updateInvoice(invoice: InvoiceModel): Observable<InvoiceModel> {
        return this.http.put<InvoiceModel>(`${this.apiUrl}/${invoice.id}`, invoice);
        // return of(invoice); // Simulating a successful response
    }

    // DELETE: remove invoice
    deleteInvoice(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
        // return of(); // Simulating a successful response
    }
}