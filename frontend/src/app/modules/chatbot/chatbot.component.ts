import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatbotService} from '../../core/services/chatbot.service';
import { ChatMessage, ChatResponse } from '../../models/chat.model';


@Component({
  selector: 'app-chatbot',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './chatbot.component.html',
  styleUrls: ['./chatbot.component.css']
})
export class ChatbotComponent implements OnInit {
  isOpen = false;
  isMinimized = false;
  unreadCount = 0;
  isTyping = false;
  currentMessage = '';
  messages: ChatMessage[] = [];

  quickActions = [
    { icon: '💰', text: 'Quel est mon solde ?' },
    { icon: '💳', text: 'Mes cartes' },
    { icon: '🧾', text: 'Dernière transaction' }
  ];

  constructor(private chatbotService: ChatbotService) {}

  ngOnInit(): void {
    this.messages = [{
      text: 'Bonjour ! Je suis l’assistant STB. Posez votre question.',
      isUser: false,
      timestamp: new Date()
    }];
  }

  toggleChat() {
    this.isOpen = !this.isOpen;
    if (this.isOpen) this.unreadCount = 0;
  }

  minimizeChat() {
    this.isMinimized = !this.isMinimized;
  }

  sendQuickAction(action: any) {
    this.sendMessageText(action.text);
  }

  onKeyPress(event: KeyboardEvent) {
    if (event.key === 'Enter' && !event.shiftKey) {
      event.preventDefault();
      this.sendMessage();
    }
  }

  sendMessage() {
    const text = this.currentMessage.trim();
    if (!text) return;
    this.sendMessageText(text);
  }

  private sendMessageText(text: string) {
    // Push message utilisateur
    this.messages.push({ text, isUser: true, timestamp: new Date() });
    this.currentMessage = '';
    this.isTyping = true;

    this.chatbotService.ask(text).subscribe({
      next: (res: ChatResponse) => {
                // réponse principale
        this.messages.push({ text: res.reply, isUser: false, timestamp: new Date() });
         // suggestions sous forme de boutons fantômes dans le flux
        if (Array.isArray(res.suggestions) && res.suggestions.length) {
          const s = 'Suggestions : ' + res.suggestions.map(x => `« ${x} »`).join('  •  ');
          this.messages.push({ text: s, isUser: false, timestamp: new Date() });
        }
        this.isTyping = false;
      },
      error: err => {
        console.error("Backend error:", err);
        const msg = (err?.error?.error) ? `Erreur serveur: ${err.error.error}` : 'Erreur lors de la requête au serveur.';
        this.messages.push({ text: msg, isUser: false, timestamp: new Date() });
        this.isTyping = false;
      }
    });
  }

  formatTime(d: Date | string) {
    const dt = typeof d === 'string' ? new Date(d) : d;
    return dt.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  }
}
