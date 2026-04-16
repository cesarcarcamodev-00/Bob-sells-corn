import { createContext, useContext, useState, useEffect, useCallback, type ReactNode } from 'react';
import { authService } from '../services/authService';

interface AuthContextType {
  client: { id: string; name: string } | null;
  isAuthenticated: boolean;
  login: (name: string) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [client, setClient] = useState<{ id: string; name: string } | null>(null);

  useEffect(() => {
    if (authService.isAuthenticated()) {
      setClient(authService.getClient());
    }
  }, []);

  const login = useCallback(async (name: string) => {
    const data = await authService.login(name);
    setClient({ id: data.clientId, name: data.clientName });
  }, []);

  const logout = useCallback(() => {
    authService.logout();
    setClient(null);
  }, []);

  return (
    <AuthContext.Provider value={{ client, isAuthenticated: !!client, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) throw new Error('useAuth must be used within AuthProvider');
  return context;
}
