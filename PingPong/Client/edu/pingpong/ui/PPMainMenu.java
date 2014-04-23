package edu.pingpong.ui;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.ArrayList;

import javax.swing.DefaultComboBoxModel;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JList;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTabbedPane;
import javax.swing.ListModel;
import javax.swing.border.LineBorder;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;

import edu.pingpong.net.GamePacket;
import edu.pingpong.net.PPConnector;
import edu.pingpong.net.PPMyStats;
import edu.pingpong.net.PPTableInfo;

/**
* This code was edited or generated using CloudGarden's Jigloo
* SWT/Swing GUI Builder, which is free for non-commercial
* use. If Jigloo is being used commercially (ie, by a corporation,
* company or business for any purpose whatever) then you
* should purchase a license for each developer using Jigloo.
* Please visit www.cloudgarden.com for details.
* Use of Jigloo implies acceptance of these licensing terms.
* A COMMERCIAL LICENSE HAS NOT BEEN PURCHASED FOR
* THIS MACHINE, SO JIGLOO OR THIS CODE CANNOT BE USED
* LEGALLY FOR ANY CORPORATE OR COMMERCIAL PURPOSE.
*/
public class PPMainMenu extends JFrame 
{
	private JTabbedPane jTabs;
	private JPanel jRankings;
	private JPanel jFreePlay;
	private JLabel jLabel2;
	private JLabel jLabel4;
	private JButton jDropTable;
	private JPanel jPanel1;
	private JLabel jLabel7;
	private JButton jJoin;
	private JButton jCreateNew;
	private JList jTables;
	private JScrollPane jScrollTables;
	private JLabel jLabel6;
	private JLabel jCnn;
	private JLabel jMyLost;
	private JLabel jMyWon;
	private JLabel jMyPlayed;
	private JLabel jMyRank;
	private JLabel jLabel5;
	private JLabel jLabel3;
	private JLabel jLabel1;
	private JLabel jRankingsLabel;
	private JList jRanks;
	private JScrollPane jRankScroll;
	private JButton jDisconnect;
	
	private int iModalResult;
	private PPConnector clientSession;
	private PPTableInfo myTable = null;
	
	private ArrayList allTables = new ArrayList();
	
	public PPMainMenu( PPConnector objCt )
	{
		initGUI();
		
		clientSession = objCt;
		iModalResult  = PPLoginFrame.mrCancel;
		
		// Add an event listener :)
		
		jTabs.addChangeListener(new ChangeListener() {
			public void stateChanged(ChangeEvent evt) {
				if ( jTabs.getSelectedIndex()  == 0 )
				{
					loadRankings();
				} else
				{
					loadTables();
				}
			}
		});
		
		loadRankings();
	}
	
	private void loadRankings()
	{
		// --- Disable all controls
		jCnn.setText( "Loading stats ..." );
		setEnabled( false );
		
		repaint();
		
		/* Get My Info */
		
		PPMyStats pp = clientSession.getMyStats();
		jMyLost.setText( String.valueOf( pp.getGamesLost() ) );
		jMyWon.setText( String.valueOf( pp.getGamesWon() ) );
		jMyRank.setText( String.valueOf( pp.getRank() ) );
		jMyPlayed.setText( String.valueOf( pp.getGamesLost() + pp.getGamesWon() ) );
				
		
		int iCnt = clientSession.getRankListPages();
		ArrayList obj = new ArrayList();
		
		for (int iX = 0; iX < iCnt; iX++ )
		{
			PPMyStats ppo[] = clientSession.getRankListPage( iX );
			
			for (int iU = 0; iU < ppo.length; iU++)
			{
				obj.add(
						String.valueOf( ppo[iU].getRank() ) + ". " +
						ppo[iU].getName() + "; Lost " +
						String.valueOf( ppo[iU].getGamesLost() ) +  "; Won " +
						String.valueOf( ppo[iU].getGamesWon() )
						);
			}
		}
		
		jRanks.setListData( obj.toArray() );
		jRanks.setPreferredSize( new java.awt.Dimension ( (int)jRanks.getSize().getWidth(), 18 * obj.size() ));
		
		
		setEnabled( true );
		jCnn.setText( "Done. You can play now!" );
	}
	
	private void loadTables()
	{
		// --- Disable all controls
		jCnn.setText( "Loading tables ..." );
		setEnabled( false );
		
		allTables.clear();
		
		repaint();
		
		PPTableInfo pt = clientSession.getMyTable();
		myTable = pt;
		
		if ( pt != null )
		{
			jDropTable.setEnabled( true );
			jCreateNew.setEnabled( false );
			jJoin.setEnabled( false );
		} else
		{
			jDropTable.setEnabled( false );
			jCreateNew.setEnabled( true );
			jJoin.setEnabled( true );			
		}
		
		/* Get My Info */
		
		int iCnt = clientSession.getTableListPages();
		ArrayList obj = new ArrayList();
		
		for (int iX = 0; iX < iCnt; iX++ )
		{
			PPTableInfo ppo[] = clientSession.getTableListPage( iX );
			
			
			for (int iU = 0; iU < ppo.length; iU++)
			{
				allTables.add( ppo[iU] );
				
				if ( ppo[iU].getStatus() == PPTableInfo.TABLE_LEFT_EMPTY )
				{
					obj.add(
							ppo[iU].getOpponent2() + "'s Table (Waiting)"
							);
				}		
				
				if ( ppo[iU].getStatus() == PPTableInfo.TABLE_RIGHT_EMPTY )
				{
					obj.add(
							ppo[iU].getOpponent1() + "'s Table (Waiting)"
							);
				}
				
				if ( ppo[iU].getStatus() == PPTableInfo.TABLE_BUSY_EMPTY )
				{
					obj.add(
							ppo[iU].getOpponent1() + " vs " +
							ppo[iU].getOpponent2() + " (In Progress)"
							);
				}				
			}
		}
		
		jTables.setListData( obj.toArray() );
		jTables.setPreferredSize( new java.awt.Dimension ( (int)jTables.getSize().getWidth(), 18 * obj.size() ));
				
			
		setEnabled( true );
		jCnn.setText( "Done. Select your table!" );
	}	
	
	public PPTableInfo getSelectedTable()
	{
		if (jDropTable.isEnabled())
			return myTable; else
				return (PPTableInfo)allTables.get( jTables.getSelectedIndex() );
	}
	
	public int getModalResult()
	{
		return iModalResult;
	}

	private void initGUI() {
		try {
			{
				jTabs = new JTabbedPane();
				getContentPane().add(jTabs);
				jTabs.setBounds(0, 0, 518, 350);
				{
					jRankings = new JPanel();
					jTabs.addTab("Rankings", null, jRankings, null);
					jRankings.setLayout(null);
					{
						jRankScroll = new JScrollPane();
						jRankings.add(jRankScroll);
						jRankScroll.setBounds(7, 21, 287, 301);
						{
							ListModel jList1Model = new DefaultComboBoxModel(
								new String[] { "Item One", "Item Two" });
							jRanks = new JList();
							jRankScroll.setViewportView(jRanks);
							jRanks.setModel(jList1Model);
							jRanks.setBounds(46, 21, 287, 296);
							jRanks.setPreferredSize(new java.awt.Dimension(281, 298));
						}
					}
					{
						jRankingsLabel = new JLabel();
						jRankings.add(jRankingsLabel);
						jRankingsLabel.setText("Rankings:");
						jRankingsLabel.setBounds(7, 5, 63, 14);
						jRankingsLabel.setFont(new java.awt.Font("Tahoma",1,11));
					}
					{
						jLabel1 = new JLabel();
						jRankings.add(jLabel1);
						jLabel1.setText("Your Status:");
						jLabel1.setBounds(350, 7, 105, 21);
						jLabel1.setFont(new java.awt.Font("Tahoma",1,14));
					}
					{
						jLabel2 = new JLabel();
						jRankings.add(jLabel2);
						jLabel2.setText("Rank:");
						jLabel2.setBounds(308, 35, 91, 14);
						jLabel2.setForeground(new java.awt.Color(255,128,64));
					}
					{
						jLabel3 = new JLabel();
						jRankings.add(jLabel3);
						jLabel3.setText("Games Played:");
						jLabel3.setBounds(308, 49, 91, 14);
					}
					{
						jLabel4 = new JLabel();
						jRankings.add(jLabel4);
						jLabel4.setText("Games Won:");
						jLabel4.setBounds(308, 63, 91, 14);
						jLabel4.setForeground(new java.awt.Color(0,0,255));
					}
					{
						jLabel5 = new JLabel();
						jRankings.add(jLabel5);
						jLabel5.setText("Games Lost:");
						jLabel5.setBounds(308, 77, 91, 14);
						jLabel5.setForeground(new java.awt.Color(255,0,0));
					}
					{
						jMyRank = new JLabel();
						jRankings.add(jMyRank);
						jMyRank.setText("---");
						jMyRank.setBounds(413, 35, 63, 14);
					}
					{
						jMyPlayed = new JLabel();
						jRankings.add(jMyPlayed);
						jMyPlayed.setText("---");
						jMyPlayed.setBounds(413, 49, 63, 14);
					}
					{
						jMyWon = new JLabel();
						jRankings.add(jMyWon);
						jMyWon.setText("---");
						jMyWon.setBounds(413, 63, 63, 14);
					}
					{
						jMyLost = new JLabel();
						jRankings.add(jMyLost);
						jMyLost.setText("---");
						jMyLost.setBounds(413, 77, 63, 14);
					}
				}
				{
					jFreePlay = new JPanel();
					jTabs.addTab("Fun Tables", null, jFreePlay, null);
					jFreePlay.setLayout(null);
					{
						jLabel6 = new JLabel();
						jFreePlay.add(jLabel6);
						jLabel6.setText("Open Tables:");
						jLabel6.setBounds(7, 7, 91, 14);
						jLabel6.setFont(new java.awt.Font("Tahoma",1,11));
					}
					{
						jScrollTables = new JScrollPane();
						jFreePlay.add(jScrollTables);
						jScrollTables.setBounds(7, 21, 364, 294);
						{
							ListModel jTablesModel = new DefaultComboBoxModel(
								new String[] { "Item One", "Item Two" });
							jTables = new JList();
							jScrollTables.setViewportView(jTables);
							jTables.setModel(jTablesModel);
							jTables.setBounds(63, 46, 354, 284);
						}
					}
					{
						jCreateNew = new JButton();
						jFreePlay.add(jCreateNew);
						jCreateNew.setText("Create ...");
						jCreateNew.setBounds(378, 21, 126, 28);
						jCreateNew.setForeground(new java.awt.Color(0,128,0));
						jCreateNew.setFont(new java.awt.Font("Tahoma",1,11));
						jCreateNew.addActionListener(new ActionListener() {
							public void actionPerformed(ActionEvent evt) {
								jCreateNewActionPerformed(evt);
							}
						});
					}
					{
						jJoin = new JButton();
						jFreePlay.add(jJoin);
						jJoin.setText("Join ...");
						jJoin.setBounds(378, 49, 126, 28);
						jJoin.setFont(new java.awt.Font("Tahoma",1,11));
						jJoin.setForeground(new java.awt.Color(0,64,128));
						jJoin.addActionListener(new ActionListener() {
							public void actionPerformed(ActionEvent evt) {
								jJoinActionPerformed(evt);
							}
						});
					}
					{
						jLabel7 = new JLabel();
						jFreePlay.add(jLabel7);
						jLabel7.setText("My Table:");
						jLabel7.setBounds(378, 266, 63, 14);
					}
					{
						jPanel1 = new JPanel();
						jFreePlay.add(jPanel1);
						jPanel1.setBounds(448, 270, 49, 7);
						jPanel1.setBorder(new LineBorder(new java.awt.Color(0,0,0), 1, false));
						jPanel1.setBackground(new java.awt.Color(0,0,0));
					}
					{
						jDropTable = new JButton();
						jFreePlay.add(jDropTable);
						jDropTable.setText("Drop Table");
						jDropTable.setBounds(378, 287, 126, 28);
						jDropTable.setForeground(new java.awt.Color(255,0,0));
						jDropTable.setFont(new java.awt.Font("Tahoma",1,11));
						jDropTable.addActionListener(new ActionListener() {
							public void actionPerformed(ActionEvent evt) {
								jDropTableActionPerformed(evt);
							}
						});
					}
				}
			}
			{
				jDisconnect = new JButton();
				getContentPane().add(jDisconnect);
				jDisconnect.setText("Disconnect");
				jDisconnect.setBounds(378, 352, 140, 26);
				jDisconnect.setFont(new java.awt.Font("Tahoma",1,11));
				jDisconnect.setEnabled(true);
				jDisconnect.addActionListener(new ActionListener() {
					public void actionPerformed(ActionEvent evt) {
						setVisible( false );
					}
				});
			}
			{
				jCnn = new JLabel();
				getContentPane().add(jCnn);
				jCnn.setText("---");
				jCnn.setBounds(7, 357, 200, 14);
				jCnn.setFont(new java.awt.Font("Tahoma",1,12));
			}
			{
				this.setTitle("Server Menu");
				getContentPane().setLayout(null);
				this.setResizable(false);
			}
			{
				this.setSize(526, 412);
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
	private void jDropTableActionPerformed(ActionEvent evt) 
	{
		if ( !clientSession.dropMyTable() )
			JOptionPane.showMessageDialog( this, "Unable to drop your table. Server error!" ); else
			{
				JOptionPane.showMessageDialog( this, "Table succesefully dropped. You can create a new one or join another's table." );
				loadTables();
			}
	}
	
	private void jJoinActionPerformed(ActionEvent evt) 
	{
		if (jTables.getSelectedIndex() == -1)
		{
			JOptionPane.showMessageDialog( this, "Please select a table to joint! Then use the Join button." );
			return;
		}
		
		iModalResult = PPLoginFrame.mrOK;
		setVisible( false );
	}
	
	private void jCreateNewActionPerformed(ActionEvent evt) 
	{
		if ( !clientSession.createMyTable() )
			JOptionPane.showMessageDialog( this, "Unable to create yourt table. Server Error!" ); else
			{
				loadTables();
			}		
	}
	
	public void processChallenge( GamePacket gp )
	{
		iModalResult = PPLoginFrame.mrAccept;		
		setVisible( false );
	}

}
