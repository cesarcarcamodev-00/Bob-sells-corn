import axios from 'axios';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5180';

interface LoginResponse {
  token: string;
  clientId: string;
  clientName: string;
  expiresAt: string;
}

const TOKEN_KEY = 'corn_token';
const EXPIRES_KEY = 'corn_expiresAt';
const CLIENT_KEY = 'corn_client';

export const authService = {
  async login(name: string): Promise<LoginResponse> {
    const { data } = await axios.post<LoginResponse>(
      `${API_URL}/api/auth/login`,
      { clientName: name }
    );

    localStorage.setItem(TOKEN_KEY, data.token);
    localStorage.setItem(EXPIRES_KEY, data.expiresAt);
    localStorage.setItem(CLIENT_KEY, JSON.stringify({
      id: data.clientId,
      name: data.clientName
    }));

    return data;
  },

  logout() {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(EXPIRES_KEY);
    localStorage.removeItem(CLIENT_KEY);
  },

  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  },

  getClient(): { id: string; name: string } | null {
    const data = localStorage.getItem(CLIENT_KEY);
    return data ? JSON.parse(data) : null;
  },

  isExpired(): boolean {
    const expires = localStorage.getItem(EXPIRES_KEY);
    if (!expires) return true;
    return Date.now() >= new Date(expires).getTime();
  },

  isAuthenticated(): boolean {
    return !!this.getToken() && !this.isExpired();
  },

  async getValidToken(): Promise<string | null> {
    const token = this.getToken();
    if (!token || this.isExpired()) {
      this.logout();
      return null;
    }
    return token;
  }
};
