# Arquitetura do AgendaUni

## Fluxo de uma operação

A tela XAML se liga ao ViewModel, que mantém o estado e os comandos. O ViewModel chama um serviço e apresenta o resultado. Os serviços validam os dados e usam contratos de repositório; os repositórios executam consultas com EF Core sobre o SQLite.

`MauiProgram.cs` registra essas dependências e aplica as migrações antes de abrir a interface. `AppDbContext` descreve as relações entre disciplinas, faltas, horários, eventos e identificadores de notificações.

## Persistência local

Cada disciplina contém faltas, horários e eventos. Eventos podem conter vários lembretes. A exclusão da disciplina remove seus registros relacionados por cascata no banco; o serviço cancela os lembretes antes de executar essa exclusão.

O caminho do banco usa `FileSystem.AppDataDirectory`. Não existe armazenamento remoto, login ou sincronização. Essa escolha permite usar a agenda offline e reduz a infraestrutura necessária para demonstrá-la.

## Notificações

`INotificationService` expõe apenas agendamento e cancelamento para os serviços de negócio. `NotificationService` integra o plugin nativo e as preferências do dispositivo. Os IDs de notificações ficam persistidos para permitir cancelamento ao editar ou excluir registros.

Ao reativar um tipo de lembrete, os agendamentos anteriores desse tipo são cancelados e os novos IDs são gravados. A configuração aguarda a operação e bloqueia alterações simultâneas durante o processamento.

Banco e sistema de notificações não compartilham uma transação. Uma falha de plataforma entre as duas operações ainda pode exigir reativar os lembretes nas configurações. Não há garantia de entrega de notificações pelo sistema operacional.

## Testes sem infraestrutura mobile

`AgendaUni.Tests` compila diretamente os arquivos de produção que não dependem de MAUI. Não existe cópia da implementação nem uma nova biblioteca de domínio para manter. Moq substitui os contratos de persistência e notificações.

A suíte cobre validações de cadastro/edição, registros ausentes, cancelamento ao editar/excluir e preservação dos lembretes quando uma edição é rejeitada. Testes de integração aplicam a migração real em SQLite em memória e verificam exclusão em cascata e atualização dos IDs de lembretes. Ela não executa a interface nem o sistema de notificações do aparelho.

## Limites assumidos

- Aplicativo de portfólio sem manutenção ativa, com stack preservada em .NET 9.
- Sem backup em nuvem, compartilhamento ou uso simultâneo em vários aparelhos.
- Validações e operações assíncronas da interface seguem a implementação original, com correções concentradas nos fluxos de lembretes.
- A migração inicial SQLite usa `CREATE TABLE IF NOT EXISTS`, mas não transforma automaticamente bancos legados com esquemas diferentes. Uma instalação limpa é o caminho da demonstração.
- Android é o alvo da demonstração; as demais configurações de plataforma não representam homologação.
