import { ChangeDetectorRef, Component, Pipe } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { InvoiceService } from '../../services/invoice.service';
import { InvoiceModel } from '../../core/models/invoice.model';
import { ToastrService } from 'ngx-toastr';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-invoice-details',
  imports: [CommonModule],
  templateUrl: './invoice-details.html',
  styleUrl: './invoice-details.css'
})
export class InvoiceDetails {

  invoiceId!: string;
  invoiceDetail!: InvoiceModel;
  constructor(
    private route: ActivatedRoute,
    private invoiceService: InvoiceService,
    private toastr: ToastrService,
    private cd: ChangeDetectorRef
  ) { }

  ngOnInit() {
    this.route.paramMap.subscribe((params) => {
      this.invoiceId = params.get('id')!;
    });
    this.getInvoiceDetails();
  }

  getInvoiceDetails() {
    this.invoiceService.getInvoiceById(this.invoiceId).subscribe({
      next: (data) => { this.invoiceDetail = data; this.cd.detectChanges(); },
      error: (err) => this.toastr.error('Error fetching invoice details', 'Error')
    })
  }
}
