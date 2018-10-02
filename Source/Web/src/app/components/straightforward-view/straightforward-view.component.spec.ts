import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StraightforwardViewComponent } from './straightforward-view.component';

describe('StraightforwardViewComponent', () => {
  let component: StraightforwardViewComponent;
  let fixture: ComponentFixture<StraightforwardViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StraightforwardViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StraightforwardViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
