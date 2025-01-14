namespace SMK_Laundry
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.manageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuManageEmployee = new System.Windows.Forms.ToolStripMenuItem();
            this.menuManageService = new System.Windows.Forms.ToolStripMenuItem();
            this.menuManagePackage = new System.Windows.Forms.ToolStripMenuItem();
            this.transactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTransactionDeposit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPrepaidPackage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewTransaction = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.manageToolStripMenuItem,
            this.transactionToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1274, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLogout,
            this.menuExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // menuLogout
            // 
            this.menuLogout.Name = "menuLogout";
            this.menuLogout.Size = new System.Drawing.Size(139, 26);
            this.menuLogout.Text = "Logout";
            this.menuLogout.Click += new System.EventHandler(this.menuLogout_Click);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(139, 26);
            this.menuExit.Text = "Exit";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // manageToolStripMenuItem
            // 
            this.manageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuManageEmployee,
            this.menuManageService,
            this.menuManagePackage});
            this.manageToolStripMenuItem.Name = "manageToolStripMenuItem";
            this.manageToolStripMenuItem.Size = new System.Drawing.Size(77, 24);
            this.manageToolStripMenuItem.Text = "Manage";
            this.manageToolStripMenuItem.Click += new System.EventHandler(this.manageToolStripMenuItem_Click);
            // 
            // menuManageEmployee
            // 
            this.menuManageEmployee.Name = "menuManageEmployee";
            this.menuManageEmployee.Size = new System.Drawing.Size(158, 26);
            this.menuManageEmployee.Text = "Employee";
            this.menuManageEmployee.Click += new System.EventHandler(this.menuManageEmployee_Click);
            // 
            // menuManageService
            // 
            this.menuManageService.Name = "menuManageService";
            this.menuManageService.Size = new System.Drawing.Size(158, 26);
            this.menuManageService.Text = "Service";
            this.menuManageService.Click += new System.EventHandler(this.menuManageService_Click);
            // 
            // menuManagePackage
            // 
            this.menuManagePackage.Name = "menuManagePackage";
            this.menuManagePackage.Size = new System.Drawing.Size(158, 26);
            this.menuManagePackage.Text = "Package";
            this.menuManagePackage.Click += new System.EventHandler(this.menuManagePackage_Click);
            // 
            // transactionToolStripMenuItem
            // 
            this.transactionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuTransactionDeposit,
            this.menuPrepaidPackage,
            this.menuViewTransaction});
            this.transactionToolStripMenuItem.Name = "transactionToolStripMenuItem";
            this.transactionToolStripMenuItem.Size = new System.Drawing.Size(98, 24);
            this.transactionToolStripMenuItem.Text = "Transaction";
            // 
            // menuTransactionDeposit
            // 
            this.menuTransactionDeposit.Name = "menuTransactionDeposit";
            this.menuTransactionDeposit.Size = new System.Drawing.Size(223, 26);
            this.menuTransactionDeposit.Text = "Transaction Deposit";
            this.menuTransactionDeposit.Click += new System.EventHandler(this.menuTransactionDeposit_Click);
            // 
            // menuPrepaidPackage
            // 
            this.menuPrepaidPackage.Name = "menuPrepaidPackage";
            this.menuPrepaidPackage.Size = new System.Drawing.Size(223, 26);
            this.menuPrepaidPackage.Text = "Prepaid Package";
            this.menuPrepaidPackage.Click += new System.EventHandler(this.menuPrepaidPackage_Click);
            // 
            // menuViewTransaction
            // 
            this.menuViewTransaction.Name = "menuViewTransaction";
            this.menuViewTransaction.Size = new System.Drawing.Size(223, 26);
            this.menuViewTransaction.Text = "View Transaction";
            this.menuViewTransaction.Click += new System.EventHandler(this.viewTToolStripMenuItem_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1274, 640);
            this.Controls.Add(this.menuStrip1);
            this.Name = "FormMain";
            this.Text = " ";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuLogout;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.ToolStripMenuItem manageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuManageEmployee;
        private System.Windows.Forms.ToolStripMenuItem menuManageService;
        private System.Windows.Forms.ToolStripMenuItem menuManagePackage;
        private System.Windows.Forms.ToolStripMenuItem transactionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuTransactionDeposit;
        private System.Windows.Forms.ToolStripMenuItem menuPrepaidPackage;
        private System.Windows.Forms.ToolStripMenuItem menuViewTransaction;
    }
}