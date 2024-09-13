import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RfAddContactComponent } from './rf-add-contact.component';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ReactiveFormsModule } from '@angular/forms';

describe('RfAddContactComponent', () => {
  let component: RfAddContactComponent;
  let fixture: ComponentFixture<RfAddContactComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RfAddContactComponent],
      imports:[HttpClientTestingModule,RouterTestingModule,ReactiveFormsModule]
    });
    fixture = TestBed.createComponent(RfAddContactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
