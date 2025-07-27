import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CustomerService } from '../../services/customer.service';

@Component({
  selector: 'app-customer-actions',
  imports: [CommonModule, RouterLink, ReactiveFormsModule],
  templateUrl: './customer-actions.html',
  styleUrl: './customer-actions.css'
})
export class CustomerActions {

  customerId!: string;
  action!: 'create' | 'read' | 'edit';
  submitted = false;
  customerRegistrationForm!: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    public fb: FormBuilder,
    private cd: ChangeDetectorRef, // ChangeDetectorRef to handle form state changes
    private toastr: ToastrService, // Toastr for notifications
    private customerService: CustomerService
  ) { }

  ngOnInit(): void {
    // Initialize the form with disabled fields
    this.customerRegistrationForm = this.fb.group({
      name: [{ value: '', disabled: true }, [Validators.required, Validators.minLength(3)]],
      email: [{ value: '', disabled: true }, [Validators.required, Validators.email]],
    });

    this.route.paramMap.subscribe((params) => {
      this.customerId = params.get('id')!;
      this.action = this.customerId ? 'read' : 'create';
      if (this.customerId) {
        this.getCustomerData(this.customerId);
        if (this.action === 'read') {
          this.customerRegistrationForm.disable();
          this.customerRegistrationForm.markAllAsTouched();
        }
      } else {
        // action is 'create'
        this.customerRegistrationForm.reset();
        this.customerRegistrationForm.enable();
      }
    });
  }

  changeAction(action: 'create' | 'read' | 'edit') {
    this.action = action;
    if (action === 'edit') {
      this.customerRegistrationForm.enable();
    }
    if (action === 'read') {
      this.customerRegistrationForm.disable();
    }
    if (action === 'create') {
      this.customerRegistrationForm.reset();
      this.customerRegistrationForm.enable();
    }
  }
  onSubmit() {
    this.submitted = true;
    this.customerRegistrationForm.markAllAsTouched(); // Ensure all fields are validated
    this.cd.detectChanges(); // Trigger change detection to update the form state
    if (!this.customerRegistrationForm.valid) {
      return;
    } else {
      this.submitCustomer();
    }
  }

  getCustomerData(customerId: string): void {
    this.customerService.getCustomerById(customerId).subscribe({
      next: (data) => {
        this.customerRegistrationForm.patchValue(data);
        // this.customerRegistrationForm.disable();
      },
      error: (error) => {
        console.error('Error fetching customer data:', error);
        this.toastr.error('Failed to load customer data', 'Error');
        setTimeout(() => { this.router.navigate(['/customers']); }, 1000);
      }
    });
  }

  submitCustomer() {
    if (this.action === 'create') {
      this.customerService.addCustomer(this.customerRegistrationForm.value).subscribe({
        next: (data) => {
          this.toastr.success('Customer created successfully', 'Success');
        },
        error: (error) => {
          this.toastr.error('Failed to create customer', 'Error');
        },
        complete: () => {
          setTimeout(() => { this.router.navigate(['/customers']); }, 1000);
        }
      });
    } else if (this.action === 'edit') {
      this.customerService.updateCustomer({ id: this.customerId, ...this.customerRegistrationForm.value }).subscribe({
        next: (data) => {
          this.toastr.success('Customer updated successfully', 'Success');
        },
        error: (error) => {
          this.toastr.error('Failed to update customer', 'Error');
        },
        complete: () => {
          setTimeout(() => { this.router.navigate(['/customers']); }, 1000);
        }
      });
    }
  }
}
