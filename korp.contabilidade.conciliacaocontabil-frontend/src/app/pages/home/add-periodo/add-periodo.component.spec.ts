import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddPeriodoComponent } from './add-periodo.component';

describe('AddPeriodoComponent', () => {
  let component: AddPeriodoComponent;
  let fixture: ComponentFixture<AddPeriodoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddPeriodoComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddPeriodoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
