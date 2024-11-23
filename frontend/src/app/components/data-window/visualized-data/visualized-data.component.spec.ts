import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VisualizedDataComponent } from './visualized-data.component';

describe('VisualizedDataComponent', () => {
  let component: VisualizedDataComponent;
  let fixture: ComponentFixture<VisualizedDataComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VisualizedDataComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VisualizedDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
