import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForgetPasswordSuccessComponent } from './forget-password-success.component';

describe('ForgetPasswordSuccessComponent', () => {
  let component: ForgetPasswordSuccessComponent;
  let fixture: ComponentFixture<ForgetPasswordSuccessComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ForgetPasswordSuccessComponent]
    });
    fixture = TestBed.createComponent(ForgetPasswordSuccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
