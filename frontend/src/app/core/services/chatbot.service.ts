import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ChatRequest, ChatResponse } from '../../models/chat.model';

@Injectable({ providedIn: 'root' })
export class ChatbotService {
  private base = 'https://localhost:7137/api/Chatbot';

  constructor(private http: HttpClient) {}

  ask(message: string): Observable<ChatResponse> {
    const body: ChatRequest = { message };
    return this.http.post<ChatResponse>(`${this.base}/ask`, body);
  }
}
