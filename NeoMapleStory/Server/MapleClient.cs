﻿using System;
using System.Security.Cryptography;
using NeoMapleStory.Core;
using NeoMapleStory.Core.Database;
using NeoMapleStory.Core.Database.Models;
using NeoMapleStory.Core.Encryption;
using NeoMapleStory.Core.IO;
using NeoMapleStory.Game.Client;
using NeoMapleStory.Packet;
using NeoMapleStory.Settings;
using SuperSocket.SocketBase;

namespace NeoMapleStory.Server
{
    public sealed class MapleClient : AppSession<MapleClient, PacketRequestInfo>, IDisposable
    {
        public MapleCipher SendCipher { get; }
        public MapleCipher RecvCipher { get; private set; }

        public byte[] SendIv { get; } = new byte[4];
        public byte[] RecvIv { get; } = new byte[4];

        public AccountModel Account { get; set; }
        public MapleCharacter Player { get; set; }

        public byte WorldId { get; set; }
        public byte ChannelId { get; set; }
        public bool IsLoggedIn { get; set; }
        public ChannelServer ChannelServer => MasterServer.Instance.ChannelServers[ChannelId];

        public bool HasHandShaked { get; set; } = false;
        public DateTime LastPongTime { get; set; }
        public int LastActionId { get; set; }



        public LoginStateType State
        {
            get { return Account.LoginState; }
            set
            {
                IsLoggedIn = value == LoginStateType.LoggedIn || value == LoginStateType.ServerTransition;
                using (var db = new NeoMapleStoryDatabase())
                {
                    db.Accounts.Attach(Account);
                    Account.LoginState = value;
                    db.SaveChanges();
                }
            }
        }

        public MapleClient()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetNonZeroBytes(SendIv);
                rng.GetNonZeroBytes(RecvIv);
            }

            unchecked
            {
                //ushort value = 0xFFFF - ServerSettings.MapleVersion;
                SendCipher = new MapleCipher((short)(0xFFFF - ServerSettings.MapleVersion), SendIv,
                    MapleCipher.CipherType.Encrypt);
                RecvCipher = new MapleCipher(ServerSettings.MapleVersion, RecvIv, MapleCipher.CipherType.Decrypt);
            }
        }

        public void Disconnect()
        {
            var chr = Player;
            if (chr != null && IsLoggedIn)
            {
                //if (chr.getTrade() != null)
                //{
                //    MapleTrade.cancelTrade(chr);
                //}
                //if (!chr.getAllBuffs().isEmpty())
                //{
                //    chr.cancelAllBuffs();
                //}

                //if (chr.getEventInstance() != null)
                //{
                //    chr.getEventInstance().playerDisconnected(chr);
                //}
                //if (NPCScriptManager.getInstance() != null)
                //{
                //    NPCScriptManager.getInstance().dispose(this);
                //}
                //if (QuestScriptManager.getInstance() != null)
                //{
                //    QuestScriptManager.getInstance().dispose(this);
                //}
                //if (!chr.isAlive())
                //{
                //    getPlayer().setHp(50, true);
                //}

                //IPlayerInteractionManager interaction = chr.; // just for safety.
                //if (interaction != null)
                //{
                //    if (interaction.isOwner(chr))
                //    {
                //        if (interaction.getShopType() == 1)
                //        {
                //            HiredMerchant hm = (HiredMerchant)interaction;
                //            hm.setOpen(true);
                //        }
                //        else if (interaction.getShopType() == 2)
                //        {
                //            for (MaplePlayerShopItem items : interaction.getItems())
                //            {
                //                if (items.getBundles() > 0)
                //                {
                //                    IItem item = items.getItem();
                //                    item.setQuantity(items.getBundles());
                //                    MapleInventoryManipulator.addFromDrop(this, item);
                //                }
                //            }
                //            interaction.removeAllVisitors(3, 1);
                //            interaction.closeShop(false); // wont happen unless some idiot hacks, hopefully ?
                //        }
                //        else if (interaction.getShopType() == 3 || interaction.getShopType() == 4)
                //        {
                //            interaction.removeAllVisitors(3, 1);
                //            interaction.closeShop(false);
                //        }
                //    }
                //    else
                //    {
                //        interaction.removeVisitor(chr);
                //    }
                //}

                //chr.AntiCheatTracker.dispose();
                //LoginServer.getInstance().removeConnectedIp(getSession().getRemoteAddress().toString());
                //try
                //{
                //    if (chr.getMessenger() != null)
                //    {
                //        MapleMessengerCharacter messengerplayer = new MapleMessengerCharacter(chr);
                //        getChannelServer().getWorldInterface().leaveMessenger(chr.getMessenger().getId(), messengerplayer);
                //        chr.setMessenger(null);
                //    }
                //}
                //catch (RemoteException e)
                //{
                //    getChannelServer().reconnectWorld();
                //    chr.setMessenger(null);
                //}
                chr.Save();
                chr.Map.RemovePlayer(chr);
                //try
                //{
                //    WorldChannelInterface wci = getChannelServer().getWorldInterface();
                //    if (chr.getParty() != null)
                //    {
                //        try
                //        {
                //            MaplePartyCharacter chrp = new MaplePartyCharacter(chr);
                //            chrp.setOnline(false);
                //            wci.updateParty(chr.getParty().getId(), PartyOperation.LOG_ONOFF, chrp);
                //        }
                //        catch (Exception e)
                //        {
                //            //log.warn("Failed removing party character. Player already removed.", e);
                //        }
                //    }
                //    if (!this.serverTransition && isLoggedIn())
                //    {
                //        wci.loggedOff(chr.getName(), chr.getId(), channel, chr.getBuddylist().getBuddyIds());
                //    }
                //    else
                //    { // Change channel
                //        wci.loggedOn(chr.getName(), chr.getId(), channel, chr.getBuddylist().getBuddyIds());
                //    }
                //    if (chr.getGuildId() > 0)
                //    {
                //        wci.setGuildMemberOnline(chr.getMGC(), false, -1);
                //    }
                //}
                //catch (RemoteException e)
                //{
                //    getChannelServer().reconnectWorld();
                //}
                //catch (NullPointerException npe)
                //{
                //}
                //catch (Exception e)
                //{
                //    log.error(getLogMessage(this, "ERROR"), e);
                //}
                //finally
                //{
                //    if (getChannelServer() != null)
                //    {
                //        getChannelServer().removePlayer(chr);
                //        if (getChannelServer().getMapleSquad(MapleSquadType.ZAKUM) != null)
                //        {
                //            if (getChannelServer().getMapleSquad(MapleSquadType.ZAKUM).getLeader() == chr)
                //            {
                //                getChannelServer().removeMapleSquad(getChannelServer().getMapleSquad(MapleSquadType.ZAKUM), MapleSquadType.ZAKUM);
                //            }
                //        }
                //    }
                //}
                //if (chr.getAllCooldowns().size() > 0)
                //{
                //    Connection con = DatabaseConnection.getConnection();
                //    for (PlayerCoolDownValueHolder cooling : chr.getAllCooldowns())
                //    {
                //        try
                //        {
                //            PreparedStatement ps = con.prepareStatement("INSERT INTO cooldowns (characterid, skillid, starttime, length) VALUES (?, ?, ?, ?)");
                //            ps.setInt(1, chr.getId());
                //            ps.setInt(2, cooling.skillId);
                //            ps.setLong(3, cooling.startTime);
                //            ps.setLong(4, cooling.length);
                //            ps.executeUpdate();
                //            ps.close();
                //        }
                //        catch (SQLException se)
                //        {
                //            se.printStackTrace();
                //        }
                //    }
                //}
            }
            if (Account != null && State != LoginStateType.ServerTransition && IsLoggedIn)
            {
                State = LoginStateType.NotLogin;
            }
        }

        #region 发送封包

        public void Send(OutPacket packet)
        {
            Send(packet.ToArray());
        }

        private void Send(byte[] packetData)
        {
            var headerData = SendCipher.GetPacketHeader(packetData.Length);
            //Console.WriteLine($"发送封包：{BitTool.GetHexStr(packetData)}");
            SendCipher.Transform(packetData);

            var finalData = new byte[packetData.Length + headerData.Length];
            Buffer.BlockCopy(headerData, 0, finalData, 0, headerData.Length);
            Buffer.BlockCopy(packetData, 0, finalData, 4, packetData.Length);

            SendRaw(finalData);
        }

        public void SendRaw(byte[] data)
        {
            Send(data, 0, data.Length);
        }

        #endregion

        protected override void OnSessionStarted()
        {
            TimerManager.Instance.RepeatTask(() =>
            {
                if (HasHandShaked)
                {
                    DateTime pingTime = DateTime.Now;
                    Send(PacketCreator.Ping());
                    TimerManager.Instance.RunOnceTask(() =>
                    {
                        if ((pingTime - LastPongTime).Seconds>10)
                        {
                            if (SocketSession.AppSession.Connected)
                                Close();
                        }
                    }, 15 * 1000);
                }
            }, 30 * 1000);

            base.OnSessionStarted();
        }

        public void Dispose()
        {

        }
    }
}