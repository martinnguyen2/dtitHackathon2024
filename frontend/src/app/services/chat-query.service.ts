import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChatQueryService {

  constructor(private http: HttpClient) { }

  public postQuery(input : string) : Observable<string>{
    return this.http.post<string>(environment.baseUrl + "/api/ChatPrompt/submit",{"prompt" : input});
  }
}
