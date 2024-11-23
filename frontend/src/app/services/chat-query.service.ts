import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../enviroments/environment.js';
import { ChatQueryModel } from '../models/chat-query.model';
import { ChatQueryResponseModel } from '../models/chat-query-response.model';

@Injectable({
  providedIn: 'root'
})
export class ChatQueryService {

  constructor(private http: HttpClient) {
  }

  postQuery(query: ChatQueryModel): Observable<ChatQueryResponseModel> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Accept: 'text/plain',
    });

    return this.http.post<ChatQueryResponseModel>(environment.baseUrl + "/api/ChatPrompt/submit", query, { headers });
  }
}
