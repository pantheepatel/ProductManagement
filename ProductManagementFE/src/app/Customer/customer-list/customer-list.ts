import { ChangeDetectorRef, Component, TemplateRef, ViewChild } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CustomerModel } from '../../core/models/customer.model';
import { CustomerService } from '../../services/customer.service';
import { CommonModule } from '@angular/common';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-customer-list',
  imports: [CommonModule, RouterLink],
  templateUrl: './customer-list.html',
  styleUrl: './customer-list.css'
})
export class CustomerList {
  customers: CustomerModel[] = [];
  isLoading = false;
  selectedCustomerId: string | null = null;
  @ViewChild('deleteModal') deleteModalRef!: TemplateRef<any>;

  constructor(
    private customerService: CustomerService,
    private toastr: ToastrService,
    private modalService: NgbModal,
    private router: Router,
    private cd: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadCustomers();
  }

  loadCustomers(): void {
    this.isLoading = true;
    this.customerService.getCustomers().subscribe({
      next: (data) => {
        this.customers = data;
        this.isLoading = false;
        this.cd.detectChanges(); // Ensure the view updates with new data
      },
      error: () => {
        this.toastr.error('Failed to load customers', 'Error');
        this.isLoading = false;
      }
    });
  }

  deleteCustomer(id: string): void {
    if (confirm('Are you sure you want to delete this customer?')) {
      this.customerService.deleteCustomer(id).subscribe({
        next: () => {
          this.toastr.success('Customer deleted successfully', 'Success');
          this.loadCustomers();
        },
        error: () => this.toastr.error('Failed to delete customer', 'Error')
      });
    }
  }

  editCustomer(id: string): void {
    this.router.navigate(['/customers/edit', id]);
  }

  viewCustomer(id: string): void {
    this.router.navigate(['/customers', id]);
  }

  addCustomer(): void {
    this.router.navigate(['/customers/add']);
  }

  openDeleteModal(event: Event, customerId: string): void {
    event.stopPropagation();
    this.selectedCustomerId = customerId;
    this.modalService.open(this.deleteModalRef);
  }

  confirmDelete(modalRef: NgbModalRef): void {
    if (!this.selectedCustomerId) return;
    this.customerService.deleteCustomer(this.selectedCustomerId).subscribe({
      next: () => {
        this.toastr.success('Customer deleted successfully', 'Success');
        modalRef.close();
        this.loadCustomers();
      },
      error: () => this.toastr.error('Failed to delete customer', 'Error')
    });
  }
}
