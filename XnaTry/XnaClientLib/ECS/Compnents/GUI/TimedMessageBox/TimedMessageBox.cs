using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using UtilsLib;
using UtilsLib.Consts;
using XnaClientLib.ECS.Compnents.GUI.TimedMessageBox.Style;
// ReSharper disable LoopCanBePartlyConvertedToQuery

namespace XnaClientLib.ECS.Compnents.GUI.TimedMessageBox
{
    public class TimedMessageBox : GuiComponent
    {
        #region Fields

        private readonly LinkedList<TimedLabel> messages;
        private readonly TimedLabelStyleFactory styleFactory;
        private readonly string fontAsset;
        private SpriteFont font;

        #endregion Fields

        #region Properties

        public float MaxHeight { get; set; } = 100;
        public TimeSpan MaxTime { get; set; } = TimeSpan.FromSeconds(1);
        public Vector2 Position { get; set; } = Vector2.Zero;
        public Color Color { get; set; } = Color.Orange;
        public TimedMessageBoxStyle Style { get; set; } = TimedMessageBoxStyle.None;

        #endregion Properties
        
        #region Constructor

        public TimedMessageBox(string fontAssetName)
        {
            fontAsset = fontAssetName;
            messages = new LinkedList<TimedLabel>();
            styleFactory = new TimedLabelStyleFactory();
            Subscribe(Constants.Messages.AddMessageToBox, TimedMessagesBox_AddMessageToBox);
            Subscribe(Constants.Messages.ClientAcceptedOnServer, TimedMessagesBox_ClientAcceptedOnServer);
            Subscribe(Constants.Messages.ClientDisconnected, TimedMessagesBox_ClientDisconnected);
        }

        #endregion Constructor

        #region Ems Callbacks

        private void TimedMessagesBox_AddMessageToBox(JObject jObject)
        {
            MessageFromField(jObject, Constants.Fields.Content);
        }

        private void TimedMessagesBox_ClientAcceptedOnServer(JObject jObject)
        {
            FormatMessageFromFields(jObject, "{0} Connected!", Constants.Fields.PlayerName);
        }
   
        private void TimedMessagesBox_ClientDisconnected(JObject jObject)
        {
            FormatMessageFromFields(jObject, "{0} Disconnected!", Constants.Fields.PlayerName);
        }

        #endregion

        #region API

        public void AddMessage(object message)
        {
            var item = new TimedLabel(message, MaxTime, Color, font)
            {
                Viewport = Viewport
            };
            item.Style = styleFactory.Create(item, Style);
            messages.AddLast(new LinkedListNode<TimedLabel>(item));
        }

        #endregion API

        #region GuiComponent Methods

        public override void Update(TimeSpan delta)
        {
            base.Update(delta);
            UpdateMessages(delta);
            RemoveExpiredMessages();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (messages.Count == 0)
                return;

            var currentMessages = new List<TimedLabel>(messages.Reverse());
            var totalHeight = 0f;

            foreach (var message in currentMessages)
            {
                if (totalHeight > MaxHeight)
                    break;

                totalHeight = DrawMessage(spriteBatch, message, totalHeight);
            }
        }

        public override void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>(fontAsset);
        }

        public override int DrawOrder => Constants.GUI.DrawOrder.Hud;

        public override bool IsHud => true;

        #endregion GuiComponent Methods

        #region Helper Methods

        /// <summary>
        /// Iterates over all current messages and updates their elapsed time
        /// </summary>
        private void UpdateMessages(TimeSpan delta)
        {
            var currentMessages = new List<TimedLabel>(messages);
            foreach (var message in currentMessages)
            {
                message.Elapsed += delta;
                message.Style?.Update();
            }
        }

        /// <summary>
        /// Iterates over all current messages and removes the expired ones
        /// </summary>
        private void RemoveExpiredMessages()
        {
            var currentMessages = new List<TimedLabel>(messages);
            foreach (var message in currentMessages.Where(message => message.IsExpired))
                messages.Remove(message);
        }

        /// <summary>
        /// Adds a message to the message box with the value from a field in the message
        /// </summary>
        /// <param name="jObject"></param>
        /// <param name="field"></param>
        private void MessageFromField(JObject jObject, string field)
        {
            var message = jObject.GetProp(field, string.Empty);
            if (!string.IsNullOrEmpty(message))
                AddMessage(message);
        }

        /// <summary>
        /// Adds a message to the message box formatted with values based on field names
        /// </summary>
        /// <param name="jObject">The message object</param>
        /// <param name="format">The format of the message</param>
        /// <param name="fields">Ordered array of field names to format by</param>
        private void FormatMessageFromFields(JObject jObject, string format, params string[] fields)
        {
            var args = fields.ToList().Select(f => jObject.GetProp(f, string.Empty).Trim().ToString()).ToArray<object>();
            AddMessage(string.Format(format, args));
        }

        /// <summary>
        /// Draw a label
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch for drawing</param>
        /// <param name="message">the label to draw</param>
        /// <param name="totalHeight">The height to draw the label at</param>
        /// <returns>Height for the next label to draw at</returns>
        private float DrawMessage(SpriteBatch spriteBatch, Label message, float totalHeight)
        {
            var text = message.ToString();
            spriteBatch.DrawString(font, text, Position + new Vector2(0, totalHeight), message.Color);
            var lineHeight = font.MeasureString(text).Y;
            totalHeight += lineHeight;
            return totalHeight;
        }

        #endregion Helper Methods
    }
}
