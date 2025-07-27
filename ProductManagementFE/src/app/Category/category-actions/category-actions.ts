import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CategoryService } from '../../services/category.service';
import { ToastrModule, ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-category-actions',
  imports: [CommonModule, RouterLink, ReactiveFormsModule],
  templateUrl: './category-actions.html',
  styleUrl: './category-actions.css'
})
export class CategoryActions {

  categoryId!: string;
  action!: 'create' | 'read' | 'edit';
  submitted = false;
  categoryRegistrationForm!: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    public fb: FormBuilder,
    private cd: ChangeDetectorRef, // ChangeDetectorRef to handle form state changes
    private toastr: ToastrService, // Toastr for notifications
    private categoryService: CategoryService
  ) { }

  ngOnInit(): void {

    // this.toastr.success('Success message', 'Success');

    // Initialize the form with disabled fields
    this.categoryRegistrationForm = this.fb.group({
      name: [{ value: '', disabled: true }, [Validators.required, Validators.minLength(3)]],
      description: [{ value: '', disabled: true }, [Validators.required, Validators.minLength(10)]],
    });

    this.route.paramMap.subscribe((params) => {
      this.categoryId = params.get('id')!;
      this.action = this.categoryId ? 'read' : 'create';
      if (this.categoryId) {
        this.getCategoryData(this.categoryId);
        if (this.action === 'read') {
          this.categoryRegistrationForm.disable();
          this.categoryRegistrationForm.markAllAsTouched();
        }
      } else {
        // action is 'create'
        this.categoryRegistrationForm.reset();
        this.categoryRegistrationForm.enable();
      }
    });
  }

  changeAction(action: 'create' | 'read' | 'edit') {
    this.action = action;
    if (action === 'edit') {
      this.categoryRegistrationForm.enable();
    }
    if (action === 'read') {
      this.categoryRegistrationForm.disable();
    }
    if (action === 'create') {
      this.categoryRegistrationForm.reset();
      this.categoryRegistrationForm.enable();
    }
  }

  onSubmit() {
    this.submitted = true;
    this.categoryRegistrationForm.markAllAsTouched(); // Ensure all fields are validated
    this.cd.detectChanges(); // Trigger change detection to update the form state
    if (!this.categoryRegistrationForm.valid) {
      return;
    } else {
      this.submitCategory();
    }
  }

  getCategoryData(categoryId: string): void {
    this.categoryService.getCategoryById(categoryId).subscribe({
      next: (data) => {
        this.categoryRegistrationForm.patchValue(data);
        // this.categoryRegistrationForm.disable();
      },
      error: (error) => {
        console.error('Error fetching category data:', error);
        this.toastr.error('Failed to load category data', 'Error');
        setTimeout(() => { this.router.navigate(['/categories']); }, 1000);
      }
    });
  }

  submitCategory() {
    if (this.action === 'create') {
      this.categoryService.addCategory(this.categoryRegistrationForm.value).subscribe({
        next: (data) => {
          this.toastr.success('Category created successfully', 'Success');
        },
        error: (error) => {
          this.toastr.error('Failed to create category', 'Error');
        },
        complete: () => {
          setTimeout(() => { this.router.navigate(['/categories']); }, 1000);
        }
      });
    } else if (this.action === 'edit') {
      this.categoryService.updateCategory({ id: this.categoryId, ...this.categoryRegistrationForm.value }).subscribe({
        next: (data) => {
          this.toastr.success('Category updated successfully', 'Success');
        },
        error: (error) => {
          this.toastr.error('Failed to update category', 'Error');
        },
        complete: () => {
          setTimeout(() => { this.router.navigate(['/categories']); }, 1000);
        }
      });
    }
  }
}
