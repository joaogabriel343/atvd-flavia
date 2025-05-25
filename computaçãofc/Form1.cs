using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace computaçãofc
{
    public partial class Form1 : Form
    {
        private string swiplPath = @"C:\Program Files\swipl\bin\swipl.exe";


        private string posicaoAtualSelecionada = string.Empty;
        private Button botaoJogadorClicado = null;
        public Form1()
        {
            InitializeComponent();
            panel1.BackColor = Color.ForestGreen;
            panel1.BorderStyle = BorderStyle.FixedSingle;

            this.Load += Form1_Load;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Pen linhaBranca = new Pen(Color.White, 2);
            Pen linhaAmarela = new Pen(Color.Yellow, 2);

            int panelWidth = panel1.Width;
            int panelHeight = panel1.Height;

            g.DrawRectangle(linhaBranca, 0, 0, panelWidth - 1, panelHeight - 1);

            g.DrawLine(linhaBranca, panelWidth / 2, 0, panelWidth / 2, panelHeight);

            int raioCirculo = (int)(panelHeight * 0.15);
            g.DrawEllipse(linhaBranca, (panelWidth / 2) - raioCirculo, (panelHeight / 2) - raioCirculo, raioCirculo * 2, raioCirculo * 2);

            int grandeAreaWidth = (int)(panelWidth * 0.18);
            int grandeAreaHeight = (int)(panelHeight * 0.6);
            int grandeAreaY = (panelHeight - grandeAreaHeight) / 2;
            g.DrawRectangle(linhaBranca, 0, grandeAreaY, grandeAreaWidth, grandeAreaHeight);

            int pequenaAreaWidth = (int)(panelWidth * 0.06);
            int pequenaAreaHeight = (int)(panelHeight * 0.3);
            int pequenaAreaY = (panelHeight - pequenaAreaHeight) / 2;
            g.DrawRectangle(linhaBranca, 0, pequenaAreaY, pequenaAreaWidth, pequenaAreaHeight);

            int penaltiX = (int)(grandeAreaWidth * 0.6);
            int penaltiY = panelHeight / 2;
            g.FillEllipse(linhaBranca.Brush, penaltiX - 3, penaltiY - 3, 6, 6);

            int raioMeiaLuaArco = (int)(panelHeight * 0.2);

            g.DrawRectangle(linhaBranca, panelWidth - grandeAreaWidth, grandeAreaY, grandeAreaWidth, grandeAreaHeight);

            g.DrawRectangle(linhaBranca, panelWidth - pequenaAreaWidth, pequenaAreaY, pequenaAreaWidth, pequenaAreaHeight);

            int penaltiXDireito = panelWidth - (int)(grandeAreaWidth * 0.6);
            g.FillEllipse(linhaBranca.Brush, penaltiXDireito - 3, penaltiY - 3, 6, 6);

            linhaBranca.Dispose();
            linhaAmarela.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CriarFormacao433();
            CarregarEscalacaoInicial();
        }
        private void CriarFormacao433()
        {
            int panelWidth = panel1.Width;
            int panelHeight = panel1.Height;
            int espacamentoX = panelWidth / 7;
            int centroY = panelHeight / 2;
            int offsetY = 40;

            AdicionarJogadorUI(espacamentoX, centroY, "gol");

            int defX = 2 * espacamentoX;
            AdicionarJogadorUI(defX, centroY - offsetY, "zag1"); 
            AdicionarJogadorUI(defX, centroY + offsetY, "zag2"); 
            AdicionarJogadorUI(defX, centroY - offsetY * 3, "le");
            AdicionarJogadorUI(defX, centroY + offsetY * 3, "ld");

            int meiX = 3 * espacamentoX + 20;
            AdicionarJogadorUI(meiX, centroY - offsetY * 2, "vol1"); 
            AdicionarJogadorUI(meiX, centroY, "mc");
            AdicionarJogadorUI(meiX, centroY + offsetY * 2, "vol2"); 

            int ataX = 4 * espacamentoX + 40;
            AdicionarJogadorUI(ataX, centroY - offsetY * 2, "pd");
            AdicionarJogadorUI(ataX, centroY + offsetY * 2, "pe");
            AdicionarJogadorUI(ataX, centroY, "ata");
        }

        private void AdicionarJogadorUI(int x, int y, string posicao)
        {
            Button jogador = CriarBotaoJogador(x - 20, y - 20, posicao);
            panel1.Controls.Add(jogador);
        }

        private Button CriarBotaoJogador(int x, int y, string posicao)
        {
            Button botao = new Button();
            botao.FlatStyle = FlatStyle.Flat;
            botao.FlatAppearance.BorderSize = 0;
            botao.BackColor = Color.White;
            botao.ForeColor = Color.Black;
            botao.Width = 40;
            botao.Height = 40;
            botao.Location = new Point(x, y);
            botao.Tag = posicao;

            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, botao.Width, botao.Height);
            botao.Region = new Region(path);

            botao.Click += BotaoJogador_Click;
            botao.Paint += new PaintEventHandler(this.DesenharBotaoJogador);

            return botao;
        }

        private void DesenharBotaoJogador(object sender, PaintEventArgs e)
        {
            Button btn = (Button)sender;
            Graphics g = e.Graphics;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (SolidBrush brush = new SolidBrush(btn.BackColor))
            {
                g.FillEllipse(brush, 0, 0, btn.Width, btn.Height);
            }

            using (Pen pen = new Pen(Color.LightGray, 1))
            {
                g.DrawEllipse(pen, 0, 0, btn.Width - 1, btn.Height - 1);
            }

            if (string.IsNullOrWhiteSpace(btn.Text))
            {
                using (Pen plusPen = new Pen(Color.Black, 3))
                {
                    int plusSize = btn.Width / 3;
                    int centerX = btn.Width / 2;
                    int centerY = btn.Height / 2;

                    g.DrawLine(plusPen, centerX - plusSize / 2, centerY, centerX + plusSize / 2, centerY);
                    g.DrawLine(plusPen, centerX, centerY - plusSize / 2, centerX, centerY + plusSize / 2);
                }
            }
            else
            {
                using (Brush textBrush = new SolidBrush(btn.ForeColor))
                {
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    g.DrawString(btn.Text, btn.Font, textBrush, new RectangleF(0, 0, btn.Width, btn.Height), sf);
                }
            }
        }
        private string ExecutePrologQuery(string query)
        {
            StringBuilder output = new StringBuilder();
            StringBuilder error = new StringBuilder();

            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string prologFilePath = Path.Combine(appDirectory, "atvda-7.pl");

            string escapedQuery = query.Replace("\"", "\\\"");

            string arguments = $"-s \"{prologFilePath}\" -g \"carrega, executa_query_string(\\\"{escapedQuery}\\\"), halt\"";

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = swiplPath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;

                process.OutputDataReceived += (sender, e) => { if (e.Data != null) output.AppendLine(e.Data); };
                process.ErrorDataReceived += (sender, e) => { if (e.Data != null) error.AppendLine(e.Data); };

                try
                {
                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        return $"ERRO NO PROLOG (ExitCode {process.ExitCode}):\nSaída:\n{output}\nErros:\n{error}";
                    }
                    else if (error.Length > 0 && !(error.ToString().Contains("Warning") || error.ToString().Contains("redefined")))
                    {
                        return $"ALERTA/ERRO DO PROLOG:\nSaída:\n{output}\nErros:\n{error}";
                    }
                    else
                    {
                        return output.ToString();
                    }
                }
                catch (Exception ex)
                {
                    return $"EXCEÇÃO NO C# AO INICIAR O PROCESSO PROLOG: {ex.Message}\nVerifique o caminho do SWI-Prolog: {swiplPath}";
                }
            }
        }


        private void BotaoJogador_Click(object sender, EventArgs e)
        {
            botaoJogadorClicado = sender as Button;
            if (botaoJogadorClicado == null) return;

            if (botaoJogadorClicado.Tag == null)
            {
                CustomMessageBox.Show("A posição deste botão não foi definida. Erro de configuração.", "Erro de Tag", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            posicaoAtualSelecionada = botaoJogadorClicado.Tag.ToString();

            string quotedPosicao = $"'{posicaoAtualSelecionada}'";
            string query = $"jogadores_disponiveis({quotedPosicao}, JogadoresDisponiveis), writeq(JogadoresDisponiveis)";

            query = query.Replace("\\", "");

            string result = ExecutePrologQuery(query);

            if (result.StartsWith("ERRO") || result.StartsWith("ALERTA"))
            {
                CustomMessageBox.Show(result, "Erro na Consulta Prolog", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<string> jogadoresDisponiveis = ParsePrologList(result);

            if (jogadoresDisponiveis.Count == 0)
            {
                CustomMessageBox.Show($"Não há jogadores disponíveis para a posição: {posicaoAtualSelecionada}", "Sem Jogadores", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            foreach (string jogadorNome in jogadoresDisponiveis)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(jogadorNome);
                item.Click += JogadorSelecionado_Click;
                contextMenu.Items.Add(item);
            }

            if (!string.IsNullOrEmpty(botaoJogadorClicado.Text))
            {
                ToolStripMenuItem removerItem = new ToolStripMenuItem("Remover " + botaoJogadorClicado.Text);
                removerItem.Click += RemoverJogador_Click;
                contextMenu.Items.Add(new ToolStripSeparator());
                contextMenu.Items.Add(removerItem);
            }

            contextMenu.Show(botaoJogadorClicado, new Point(botaoJogadorClicado.Width / 2, botaoJogadorClicado.Height / 2));
        }

        private void JogadorSelecionado_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item == null) return;

            string jogadorSelecionado = item.Text;

            string quotedPosicao = $"'{posicaoAtualSelecionada}'";
            string quotedJogadorAtual = $"'{botaoJogadorClicado.Text.ToLower()}'";
            string quotedJogadorSelecionado = $"'{jogadorSelecionado.ToLower()}'";

            if (!string.IsNullOrEmpty(botaoJogadorClicado.Text))
            {
                string desescalarSalvarQuery = $"desescalar({quotedPosicao}, {quotedJogadorAtual}), salva.";

                string desescalarResult = ExecutePrologQuery(desescalarSalvarQuery);
                if (desescalarResult.StartsWith("ERRO") || desescalarResult.StartsWith("ALERTA"))
                {
                    CustomMessageBox.Show(desescalarResult, "Erro ao desescalar jogador anterior", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            string escalarSalvarQuery = $"escalar({quotedPosicao}, {quotedJogadorSelecionado}), salva.";

            string escalarResult = ExecutePrologQuery(escalarSalvarQuery);
            if (escalarResult.StartsWith("ERRO") || escalarResult.StartsWith("ALERTA"))
            {
                CustomMessageBox.Show(escalarResult, "Erro ao escalar jogador", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            botaoJogadorClicado.Text = jogadorSelecionado;
            botaoJogadorClicado.BackColor = Color.Black;
            botaoJogadorClicado.ForeColor = Color.White;
            botaoJogadorClicado.Font = new Font("Arial", 7, FontStyle.Bold);
            CustomMessageBox.Show($"'{jogadorSelecionado}' escalado como {posicaoAtualSelecionada} e salvo!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RemoverJogador_Click(object sender, EventArgs e)
        {
            if (botaoJogadorClicado == null || string.IsNullOrEmpty(botaoJogadorClicado.Text)) return;

            string jogadorARemover = botaoJogadorClicado.Text.ToLower();
            string quotedJogadorARemover = $"'{jogadorARemover}'";
            string quotedPosicao = $"'{posicaoAtualSelecionada}'";

            string desescalarSalvarQuery = $"desescalar({quotedPosicao}, {quotedJogadorARemover}), salva.";

            string desescalarResult = ExecutePrologQuery(desescalarSalvarQuery);

            if (desescalarResult.StartsWith("ERRO") || desescalarResult.StartsWith("ALERTA"))
            {
                CustomMessageBox.Show(desescalarResult, "Erro ao remover jogador", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            botaoJogadorClicado.Text = "";
            botaoJogadorClicado.BackColor = Color.White;
            botaoJogadorClicado.ForeColor = Color.Black;
            botaoJogadorClicado.Font = new Font("Microsoft Sans Serif", 8);
            botaoJogadorClicado.Invalidate();

            CustomMessageBox.Show($"Jogador '{jogadorARemover}' removido da posição {posicaoAtualSelecionada} e salvo!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private List<string> ParsePrologList(string prologListString)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrWhiteSpace(prologListString) || prologListString.Trim() == "[]")
            {
                return list;
            }

            string cleanedString = prologListString.Trim().Trim('[', ']', '\n', '\r');
            if (string.IsNullOrEmpty(cleanedString)) return list;

            string[] items = cleanedString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in items)
            {
                list.Add(item.Trim());
            }
            return list;
        }
        private void CarregarEscalacaoInicial()
        {
            string query = "carrega, listar_escalacao.";

            string result = ExecutePrologQuery(query);

            if (result.StartsWith("ERRO") || result.StartsWith("ALERTA"))
            {
                CustomMessageBox.Show(result, "Erro ao carregar escalação inicial", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<(string Posicao, string Jogador)> escalacao = ParsePrologEscalacaoList(result);

            foreach (Control control in panel1.Controls)
            {
                if (control is Button button && button.Tag is string buttonTag)
                {
                    string buttonPosicao = buttonTag;
                    var jogadorEscalado = escalacao.FirstOrDefault(item => item.Posicao.ToLower() == buttonPosicao.ToLower());

                    if (jogadorEscalado.Jogador != null && !string.IsNullOrEmpty(jogadorEscalado.Jogador))
                    {
                        button.Text = char.ToUpper(jogadorEscalado.Jogador[0]) + jogadorEscalado.Jogador.Substring(1);
                        button.BackColor = Color.Black;
                        button.ForeColor = Color.White;
                        button.Font = new Font("Arial", 7, FontStyle.Bold);
                    }
                    else
                    {
                        button.Text = "";
                        button.BackColor = Color.White;
                        button.ForeColor = Color.Black;
                        button.Font = new Font("Microsoft Sans Serif", 8);
                    }
                }
            }
            panel1.Invalidate();
        }
        private List<(string Posicao, string Jogador)> ParsePrologEscalacaoList(string prologListString)
        {
            List<(string Posicao, string Jogador)> list = new List<(string Posicao, string Jogador)>();
            if (string.IsNullOrWhiteSpace(prologListString))
            {
                return list;
            }

            string[] lines = prologListString.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"posicao_escalada\(([^,]+),\s*([^)]+)\)\.");

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (trimmedLine.StartsWith("posicao_escalada"))
                {
                    System.Text.RegularExpressions.Match match = regex.Match(trimmedLine);
                    if (match.Success && match.Groups.Count == 3)
                    {
                        string posicao = match.Groups[1].Value.Trim('\'');
                        string jogador = match.Groups[2].Value.Trim('\'');
                        list.Add((posicao, jogador));
                    }
                }
            }
            return list;
        }
    }
}
