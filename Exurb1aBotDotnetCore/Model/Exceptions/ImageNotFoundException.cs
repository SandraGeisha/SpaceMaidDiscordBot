using System;

namespace Exurb1aBot.Model.Exceptions {
    class ImageNotFoundException:Exception{
        public ImageNotFoundException():base("Image not found") {}
    }
}
