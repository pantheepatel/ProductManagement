import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ProductService } from '../../services/product.service';
import { CategoryDTO } from '../../core/models/category.model';
import { CategoryService } from '../../services/category.service';

@Component({
  selector: 'app-product-actions',
  standalone: true,
  imports: [CommonModule, RouterLink, ReactiveFormsModule],
  templateUrl: './product-actions.html',
  styleUrl: './product-actions.css'
})
export class ProductActions {
  productId!: string;
  action!: 'create' | 'read' | 'edit';
  submitted = false;
  categories: CategoryDTO[] = [];
  productRegistrationForm!: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    public fb: FormBuilder,
    private cd: ChangeDetectorRef,
    private toastr: ToastrService,
    private productService: ProductService,
    private categoryService: CategoryService
  ) { }

  ngOnInit(): void {
    this.productRegistrationForm = this.fb.group({
      name: [{ value: '', disabled: true }, [Validators.required, Validators.minLength(3)]],
      description: [{ value: '', disabled: true }, [Validators.required, Validators.minLength(10)]],
      basePrice: [{ value: '', disabled: true }, [Validators.required, Validators.min(0)]],
      categoryId: [{ value: '', disabled: true }, [Validators.required]],
      tax: [{ value: '', disabled: true }, [Validators.required, Validators.min(0)]],
      prices: this.fb.array([], { validators: [this.noDateOverlap] }),
      currentPrice: [{ value: '', disabled: true }, [Validators.required, Validators.min(0)]]
    });

    this.route.paramMap.subscribe((params) => {
      this.productId = params.get('id')!;
      this.action = this.productId ? 'read' : 'create';
      this.prices.valueChanges.subscribe(() => this.calculateCurrentPrice());
      if (this.productId) {
        this.getProductData(this.productId);
        if (this.action === 'read') {
          this.productRegistrationForm.disable();
          this.productRegistrationForm.markAllAsTouched();
        }
      } else {
        this.productRegistrationForm.reset();
        this.productRegistrationForm.enable();
        this.productRegistrationForm.get('currentPrice')?.disable();
      }
      this.calculateCurrentPrice();
    });

    this.noDateOverlap = this.noDateOverlap.bind(this);

    this.loadCategories();
  }

  loadCategories() {
    this.categoryService.getCategories().subscribe({
      next: (data) => this.categories = data,
      error: () => this.toastr.error('Failed to load categories', 'Error')
    });
  }

  calculateCurrentPrice(): void {
    const prices = this.prices.controls;
    const today = new Date();

    // Find active seasonal price
    const activePriceGroup = prices.find(ctrl => {
      const start = new Date(ctrl.get('startDate')?.value);
      const end = new Date(ctrl.get('endDate')?.value);
      return start <= today && end >= today;
    });

    const seasonalPrice = activePriceGroup?.get('seasonalPrice')?.value;
    const basePrice = this.productRegistrationForm.get('basePrice')?.value || 0;
    const tax = this.productRegistrationForm.get('tax')?.value || 0;

    const priceToUse = seasonalPrice ?? basePrice;
    const priceWithTax = priceToUse * (1 + tax / 100);

    this.productRegistrationForm.get('currentPrice')?.setValue(Number(priceWithTax.toFixed(2)));
  }

  changeAction(action: 'create' | 'read' | 'edit') {
    this.action = action;
    if (action === 'edit') {
      this.productRegistrationForm.enable();
    }
    if (action === 'read') {
      this.productRegistrationForm.disable();
    }
    if (action === 'create') {
      this.productRegistrationForm.reset();
      this.productRegistrationForm.enable();
    }
  }

  get prices(): FormArray {
    return this.productRegistrationForm.get('prices') as FormArray;
  }

  createSeasonalPriceGroup(): FormGroup {
    return this.fb.group({
      seasonalPrice: ['', [Validators.required, Validators.min(0)]],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required]
    });
  }

  newRow(): void {
    if (this.isReadOnlyMode()) {
      return;
    }
    this.prices.push(this.createSeasonalPriceGroup());
  }

  onSubmit() {
    this.submitted = true;
    this.productRegistrationForm.markAllAsTouched();
    this.cd.detectChanges();
    if (!this.productRegistrationForm.valid) return;

    this.submitProduct();
  }

  isReadOnlyMode(): boolean {
    return this.action === 'read';
  }

  getProductData(productId: string): void {
    this.productService.getProductById(productId).subscribe({
      next: (data) => {
        this.productRegistrationForm.patchValue(data);
        const pricesArray = this.fb.array([]);
        if (data.prices && Array.isArray(data.prices)) {
          data.prices.forEach(price => {
            const group = this.fb.group({
              seasonalPrice: [price.seasonalPrice],
              startDate: [price.startDate],
              endDate: [price.endDate]
            });
            (pricesArray as FormArray).push(group);
          });

          pricesArray.setValidators([this.noDateOverlap]);
        }
        this.productRegistrationForm.setControl('prices', pricesArray);
        pricesArray.updateValueAndValidity();
        this.cd.detectChanges();
        this.calculateCurrentPrice();
      },
      error: () => {
        this.toastr.error('Failed to load product data', 'Error');
        setTimeout(() => this.router.navigate(['/products']), 1000);
      }
    });
  }

  noDateOverlap(control: AbstractControl): ValidationErrors | null {
    const prices = (control as FormArray).controls;
    const ranges = prices.map((group, index) => {
      const start = new Date(group.get('startDate')?.value);
      const end = new Date(group.get('endDate')?.value);

      // Add error if start >= end
      if (start && end && start >= end) {
        group.get('startDate')?.setErrors({ invalidRange: true });
        group.get('endDate')?.setErrors({ invalidRange: true });
      } else {
        group.get('startDate')?.setErrors(null);
        group.get('endDate')?.setErrors(null);
      }

      return { start, end, index };
    });

    // Check overlap
    for (let i = 0; i < ranges.length; i++) {
      for (let j = i + 1; j < ranges.length; j++) {
        if (
          ranges[i].start <= ranges[j].end &&
          ranges[j].start <= ranges[i].end
        ) {
          return { overlap: true };
        }
      }
    }

    return null;
  }

  submitProduct() {
    const formData = this.productRegistrationForm.getRawValue();
    console.log("prices:",formData)
    if (this.action === 'create') {
      this.productService.addProduct(formData).subscribe({
        next: () => this.toastr.success('Product created successfully', 'Success'),
        error: () => this.toastr.error('Failed to create product', 'Error'),
        complete: () => setTimeout(() => this.router.navigate(['/products']), 1000)
      });
    } else if (this.action === 'edit') {
      this.productService.updateProduct({ id: this.productId, ...formData }).subscribe({
        next: () => this.toastr.success('Product updated successfully', 'Success'),
        error: () => this.toastr.error('Failed to update product', 'Error'),
        complete: () => setTimeout(() => this.router.navigate(['/products']), 1000)
      });
    }
  }
}
