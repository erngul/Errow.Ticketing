import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SeatComponentComponent } from './seat-component.component';

describe('SeatComponentComponent', () => {
  let component: SeatComponentComponent;
  let fixture: ComponentFixture<SeatComponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SeatComponentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SeatComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
