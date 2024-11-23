import { TestBed } from '@angular/core/testing';

import { ChatQueryService } from './chat-query.service';

describe('ChatQueryService', () => {
  let service: ChatQueryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChatQueryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
