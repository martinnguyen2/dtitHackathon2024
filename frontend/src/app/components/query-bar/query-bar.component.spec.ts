import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QueryBarComponent } from './query-bar.component';

describe('QueryBarComponent', () => {
  let component: QueryBarComponent;
  let fixture: ComponentFixture<QueryBarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QueryBarComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QueryBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
