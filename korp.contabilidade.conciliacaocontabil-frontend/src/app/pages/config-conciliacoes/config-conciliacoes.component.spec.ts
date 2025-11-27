import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfigConciliacoesComponent } from './config-conciliacoes.component';

describe('ConfigConciliacoesComponent', () => {
  let component: ConfigConciliacoesComponent;
  let fixture: ComponentFixture<ConfigConciliacoesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConfigConciliacoesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ConfigConciliacoesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
