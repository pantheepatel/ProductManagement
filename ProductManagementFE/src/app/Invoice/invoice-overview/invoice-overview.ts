import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-invoice-overview',
  imports: [CommonModule, RouterLink],
  templateUrl: './invoice-overview.html',
  styleUrl: './invoice-overview.css'
})
export class InvoiceOverview {
  @Input() userId!: string;
  invoices: any[] = [
    {
      id: 1,
      details: 'Invoice details for user',
      totalAmount: 100.00
    },
    {
      id: 2,
      details: 'Another invoice details for user',
      totalAmount: 200.00
    }]; // This should be replaced with the actual type of invoices
  constructor() { }
  ngOnInit() {
    // Fetch invoices based on userId
    // this.fetchInvoices();
    console.log(`Fetching invoices for user: ${this.userId}`);
  }
}
