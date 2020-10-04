using Discord;
using Exurb1aBot.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exurb1aBot.Util.Workers {
    public class VCWorkerService {
        public List<VCWorker> Workers { get; set; }
        private readonly IScoreRepsitory _scoreRepo;
        private readonly Random _random;

        public VCWorkerService(IScoreRepsitory scoreRepo) {
            Workers = new List<VCWorker>();
            _scoreRepo = scoreRepo;
            _random = new Random();
        }

        private bool ChannelHasWorker(IVoiceChannel channel) {
            return  Workers.FirstOrDefault(w => w.AudioChannel == channel) != null;
        }

        public async Task AddWorkerToChannel(IVoiceChannel channel) {
            if (!ChannelHasWorker(channel)) {
                VCWorker worker = new VCWorker(channel);
                worker.VCWorkerCompletedEvent += Worker_VCWorkerCompletedEvent;
                worker.AwardVCPointsEvent += Worker_AwardVCPointsEvent;
                Workers.Add(worker);
                worker.Start();
            }
        }

        private void Worker_AwardVCPointsEvent(IGuildUser user) {
            _scoreRepo.Increment(user, Enums.ScoreType.VC, _random.Next(3, 5));
        }

        private void Worker_VCWorkerCompletedEvent(VCWorker worker) {
            Workers.Remove(worker);
        }
    }
}
