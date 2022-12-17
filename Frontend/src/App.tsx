import React from "react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import "./fonts.css";
import "@fontsource/roboto/300.css";
import "@fontsource/roboto/400.css";
import "@fontsource/roboto/500.css";
import "@fontsource/roboto/700.css";
import "./poe.css";
import RecipeGenerator from "./RecipeGenerator";
import { Container } from "@mui/material";

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 0,
      cacheTime: 0,
    },
  },
});

const App: React.FC = () => {
  return (
    <QueryClientProvider client={queryClient}>
      <Container maxWidth="sm">
        <RecipeGenerator />
      </Container>
    </QueryClientProvider>
  );
};

export default App;
