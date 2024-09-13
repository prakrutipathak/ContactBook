import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RfModifyContactComponent } from './rf-modify-contact.component';
import { RouterTestingModule } from '@angular/router/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { DatePipe } from '@angular/common';

describe('RfModifyContactComponent', () => {
  let component: RfModifyContactComponent;
  let fixture: ComponentFixture<RfModifyContactComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RfModifyContactComponent],
      imports:[HttpClientTestingModule,RouterTestingModule,ReactiveFormsModule],
      providers:[DatePipe]
    });
    fixture = TestBed.createComponent(RfModifyContactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
