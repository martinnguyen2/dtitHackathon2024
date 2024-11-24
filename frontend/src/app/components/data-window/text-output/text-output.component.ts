import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { ChatQueryResponseModel } from '../../../models/chat-query-response.model';

@Component({
  selector: 'app-text-output',
  imports: [],
  templateUrl: './text-output.component.html',
  styleUrl: './text-output.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TextOutputComponent {
  @Input() set data(data: ChatQueryResponseModel | undefined) {
    this.text = data?.textOutput || '';
  };
  text = '';

}
