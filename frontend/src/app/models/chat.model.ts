// chat.model.ts
export interface ChatRequest {
  message: string;
}

export interface ChatResponse {
  reply: string;
  suggestions: string[];
}

export interface ChatMessage {
  text: string;
  isUser: boolean;
  timestamp: Date;
}
