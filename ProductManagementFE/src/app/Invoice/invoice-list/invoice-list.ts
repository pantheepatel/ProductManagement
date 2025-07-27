import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { InvoiceCreateDTO, InvoiceItemCreateDTO, InvoiceItemModel, InvoiceModel } from '../../core/models/invoice.model';
import { InvoiceService } from '../../services/invoice.service';
import { NgbModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { Router, RouterLink } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CustomerModel } from '../../core/models/customer.model';
import { ProductModel } from '../../core/models/product.model';
import { ProductService } from '../../services/product.service';
import { CustomerService } from '../../services/customer.service';

@Component({
  selector: 'app-invoice-list',
  imports: [ReactiveFormsModule, NgbModule, CommonModule, RouterLink],
  templateUrl: './invoice-list.html',
  styleUrl: './invoice-list.css'
})
export class InvoiceList {
  invoices!: InvoiceModel[];
  invoiceItems: any[] = [];
  selectedInvoiceId: string = '';
  invoiceForm!: FormGroup;
  customers!: CustomerModel[];
  products!: ProductModel[];
  @ViewChild('deleteModal') deleteModal: any;

  constructor(
    private invoiceService: InvoiceService,
    private productService: ProductService,
    private customerService: CustomerService,
    private modalService: NgbModal,
    private router: Router,
    private toastr: ToastrService,
    private fb: FormBuilder,
    private cd: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.invoiceForm = this.fb.group({
      customerId: ['', Validators.required],
      productId: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]]
    });
    this.getInvoices();
    // this.loadInitialData();
  }

  getInvoices(): void {
    this.invoiceService.getInvoices().subscribe({
      next: (data) => {
        this.invoices = data,
          this.loadInitialData();
      },
      error: (error) => {
        this.toastr.error('Error fetching invoices', 'Error');
      }
    });
  }

  loadInitialData(): void {
    this.customerService.getCustomers().subscribe({
      next: (data) => { this.customers = data; this.cd.detectChanges(); },
      error: (error) => this.toastr.error('Error fetching customers', 'Error')
    })
    this.productService.getProducts().subscribe({
      next: (data) => { this.products = data; this.cd.detectChanges(); },
      error: (error) => this.toastr.error('Error fetching products', 'Error')
    })
  }

  openDeleteModal(event: MouseEvent, id: string): void {
    event.stopPropagation(); // prevent row click
    this.selectedInvoiceId = id;
    this.modalService.open(this.deleteModal, { centered: true });
  }

  confirmDelete(modalRef: any): void {
    console.log("delete invoice id:", this.selectedInvoiceId);
    this.invoiceService.deleteInvoice(this.selectedInvoiceId).subscribe({
      next: () => {
        this.invoices = this.invoices.filter(cat => cat.id !== this.selectedInvoiceId);
        modalRef.close();
      },
      error: (error) => {
        this.toastr.error('Error deleting invoice', 'Error');
        modalRef.dismiss();
      },
      complete: () => {
        this.toastr.success('Invoice deleted successfully', 'Success');
        modalRef.close();
      }
    });
  }

  addInvoiceItem(): void {
    const { productId, quantity } = this.invoiceForm.value;
    const product = this.products.find(p => p.id == productId);
    if (!product) return;

    const existingItem = this.invoiceItems.find(item => item.productId == product.id);

    if (existingItem) {
      // Update quantity
      existingItem.quantity += quantity;
    } else {
      // Add new item
      this.invoiceItems.push({
        productId: product.id,
        productName: product.name,
        quantity,
        price: product.prices,
        tax: product.tax
      });
    }
  }

  removeItem(index: number): void {
    this.invoiceItems.splice(index, 1);
  }

  createInvoice(): void {
    if (!this.invoiceForm.valid || !this.invoiceForm.value.customerId) {
      this.toastr.warning('Please select a customer before creating an invoice.');
      return;
    }

    const { customerId } = this.invoiceForm.value;
    const date = new Date();

    // Prepare DTO items (only productId and quantity required)
    const dtoItems: InvoiceItemCreateDTO[] = this.invoiceItems.map(item =>
      new InvoiceItemCreateDTO(item.productId, item.quantity)
    );

    const invoiceDTO = new InvoiceCreateDTO(customerId, date, dtoItems);
    console.log("Creating invoice with DTO:", invoiceDTO);

    this.invoiceService.addInvoice(invoiceDTO).subscribe({
      next: () => {
        this.toastr.success('Invoice created successfully.');
        this.invoiceItems = [];
        this.invoiceForm.reset({ quantity: 1 });
        this.getInvoices(); // Refresh the invoice list with updated totals
      },
      error: () => {
        this.toastr.error('Failed to create invoice.');
      }
    });
  }

  getCustomerName(customerId: string): string {
    return this.customers.find(c => c.id === customerId)?.name || 'Unknown';
  }

}
