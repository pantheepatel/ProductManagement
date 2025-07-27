import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable, of } from 'rxjs';
import { CustomerModel, CustomerCreateModel } from '../core/models/customer.model';

@Injectable({ providedIn: 'root' })
export class CustomerService {

  private apiUrl = `${environment.baseUrl}api/Customer`;

  constructor(private http: HttpClient) {}

  // GET: list all customers
  getCustomers(): Observable<CustomerModel[]> {
    return this.http.get<CustomerModel[]>(`${this.apiUrl}`);
    // return of([
    //   { id: 'CUST-001', name: 'Alice Johnson', email: 'alice@example.com' },
    //   { id: 'CUST-002', name: 'Bob Smith', email: 'bob@example.com' },
    //   { id: 'CUST-003', name: 'Charlie Davis', email: 'charlie@example.com' }
    // ]);
  }

  // GET: customer by ID
  getCustomerById(id: CustomerModel['id']): Observable<CustomerModel> {
    return this.http.get<CustomerModel>(`${this.apiUrl}/${id}`);
    // return of({ id: id, name: 'Mock Customer', email: 'mock@example.com' });
  }

  // POST: add a new customer
  addCustomer(customer: CustomerCreateModel): Observable<CustomerModel> {
    return this.http.post<CustomerModel>(`${this.apiUrl}`, customer);
    // return of({
    //   id: 'CUST-NEW',
    //   ...customer
    // });
  }

  // PUT: update an existing customer
  updateCustomer(customer: CustomerModel): Observable<CustomerModel> {
    return this.http.put<CustomerModel>(`${this.apiUrl}/${customer.id}`, customer);
    // return of(customer);
  }

  // DELETE: delete a customer by ID
  deleteCustomer(id: CustomerModel['id']): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
    // return of();
  }
}
