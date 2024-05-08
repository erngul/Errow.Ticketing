import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateSeatsComponent } from './create-seats.component';

describe('CreateSeatsComponent', () => {
  let component: CreateSeatsComponent;
  let fixture: ComponentFixture<CreateSeatsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateSeatsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateSeatsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
