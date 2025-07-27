import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvoiceOverview } from './invoice-overview';

describe('InvoiceOverview', () => {
  let component: InvoiceOverview;
  let fixture: ComponentFixture<InvoiceOverview>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InvoiceOverview]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InvoiceOverview);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
