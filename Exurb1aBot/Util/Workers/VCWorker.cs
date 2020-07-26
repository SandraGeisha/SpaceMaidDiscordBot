using Discord;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Exurb1aBot.Util.Workers {
    public class VCWorker{
        private readonly BackgroundWorker _worker;
        private static readonly int checkPeriod = 300000;

        public delegate void VCWorkerCompleted(VCWorker worker);
        public event VCWorkerCompleted VCWorkerCompletedEvent;

        public delegate void AwardVCPoints(IGuildUser user);
        public event AwardVCPoints AwardVCPointsEvent;

        public IVoiceChannel AudioChannel { get; private set; }

        public VCWorker(IVoiceChannel channel) {
            _worker = new BackgroundWorker();
            AudioChannel = channel;
            _worker.DoWork += Worker_DoWork;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
            _worker.ProgressChanged += _worker_ProgressChanged;
        }

        private void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            AwardVCPointsEvent.Invoke((IGuildUser)(e.UserState));
        }

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            VCWorkerCompletedEvent.Invoke(this);
        }

        public void Start() {
            _worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e) {
            while (true) {
                IReadOnlyCollection<IGuildUser> users = AudioChannel.GetUsersAsync().ToListAsync().Result[0];
                IEnumerable<IGuildUser> nonBots = users.Where(u => !u.IsBot).ToList();

                IEnumerable<IGuildUser> filteredUser = nonBots.Where(u => {
                    bool cond = !u.IsSelfDeafened;
                    return cond && !u.IsSelfMuted;
                }).ToList();

                if (nonBots.Count() == 0) {
                    VCWorkerCompletedEvent.Invoke(this);
                    _worker.CancelAsync();
                    return;
                }

                if (filteredUser.Count() >= 2) {
                    foreach (IGuildUser user in filteredUser) {
                        AwardVCPointsEvent.Invoke((IGuildUser)(user));
                    }
                }

                Thread.Sleep(checkPeriod);
            }
        }
    }
}
