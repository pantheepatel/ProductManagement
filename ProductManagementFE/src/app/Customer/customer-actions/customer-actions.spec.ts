import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerActions } from './customer-actions';

describe('CustomerActions', () => {
  let component: CustomerActions;
  let fixture: ComponentFixture<CustomerActions>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerActions]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomerActions);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
