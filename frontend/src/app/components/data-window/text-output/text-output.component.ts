import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { ChatQueryResponseModel } from '../../../models/chat-query-response.model';
import { ChatQueryService } from '../../../services/chat-query.service';

@Component({
  selector: 'app-text-output',
  imports: [],
  templateUrl: './text-output.component.html',
  styleUrl: './text-output.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TextOutputComponent {
  @Input() set data(data: ChatQueryResponseModel | undefined) {
    this.prompt = this.chatquery.prompt;
    this.textArray.push(this.prompt);
    this.textArray.push(data?.textOutput || '');
  };
  textArray: string [] = [];
  prompt: string = '';
  constructor(private chatquery: ChatQueryService) {}
}
